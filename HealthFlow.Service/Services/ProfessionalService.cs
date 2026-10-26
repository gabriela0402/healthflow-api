using System.Net.Mail;
using HealthFlow.Model.DTOs.Professionals;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HealthFlow.Service.Services;

public class ProfessionalService : IProfessionalService
{
    private readonly IProfessionalRepository _professionalRepository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public ProfessionalService(
        IProfessionalRepository professionalRepository,
        ISpecialtyRepository specialtyRepository,
        IUserRepository userRepository)
    {
        _professionalRepository = professionalRepository;
        _specialtyRepository = specialtyRepository;
        _userRepository = userRepository;
    }

    public async Task<ProfessionalResponse> CreateAsync(
        CreateProfessionalRequest request,
        CancellationToken cancellationToken = default)
    {
        var fullName = ValidateName(request.FullName);
        var registrationNumber = ValidateRegistration(request.RegistrationNumber);
        var email = ValidateEmail(request.Email);
        ValidatePassword(request.Password);

        if (request.SpecialtyId <= 0)
        {
            throw new BusinessRuleException("A especialidade é obrigatória.");
        }

        await EnsureRegistrationIsFreeAsync(registrationNumber, null, cancellationToken);
        await EnsureSpecialtyExistsAsync(request.SpecialtyId, cancellationToken);
        await EnsureEmailIsFreeAsync(email, cancellationToken);

        var user = CreateUser(fullName, email, request.Password);

        var professional = new Professional(
            fullName,
            registrationNumber,
            request.SpecialtyId,
            email);

        await _userRepository.AddAsync(user, cancellationToken);
        await _professionalRepository.AddAsync(professional, cancellationToken);

        await _professionalRepository.SaveChangesAsync(cancellationToken);

        var created = await _professionalRepository.GetByIdAsync(
            professional.Id,
            cancellationToken);

        return ProfessionalResponse.FromEntity(created!);
    }

    public async Task<ProfessionalResponse> UpdateAsync(
        int id,
        UpdateProfessionalRequest request,
        CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessRuleException("Profissional não encontrado.");

        var fullName = ValidateName(request.FullName);
        var registrationNumber = ValidateRegistration(request.RegistrationNumber);

        await EnsureRegistrationIsFreeAsync(registrationNumber, id, cancellationToken);
        await EnsureSpecialtyExistsAsync(request.SpecialtyId, cancellationToken);

        var newPassword = string.IsNullOrWhiteSpace(request.NewPassword)
            ? null
            : request.NewPassword;

        var requestedEmail = string.IsNullOrWhiteSpace(request.Email)
            ? null
            : ValidateEmail(request.Email);

        if (string.IsNullOrEmpty(professional.Email))
        {
            if (requestedEmail is not null)
            {
                if (newPassword is null)
                {
                    throw new BusinessRuleException(
                        "Informe uma senha inicial para criar o acesso do profissional.");
                }

                ValidatePassword(newPassword);
                await EnsureEmailIsFreeAsync(requestedEmail, cancellationToken);

                await _userRepository.AddAsync(
                    CreateUser(fullName, requestedEmail, newPassword),
                    cancellationToken);

                professional.SetEmail(requestedEmail);
            }
        }
        else
        {
            if (requestedEmail is not null && requestedEmail != professional.Email)
            {
                throw new BusinessRuleException(
                    "O e-mail do profissional é o login e não pode ser alterado.");
            }

            var user = await _userRepository.GetByEmailAsync(
                professional.Email,
                cancellationToken);

            if (user is not null)
            {
                user.SetName(fullName);

                if (newPassword is not null)
                {
                    ValidatePassword(newPassword);
                    user.SetPasswordHash(_passwordHasher.HashPassword(user, newPassword));
                }
            }
        }

        professional.Update(fullName, registrationNumber, request.SpecialtyId);

        await _professionalRepository.SaveChangesAsync(cancellationToken);

        var updated = await _professionalRepository.GetByIdAsync(id, cancellationToken);

        return ProfessionalResponse.FromEntity(updated!);
    }

    public async Task<ProfessionalResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new BusinessRuleException(
                "O identificador do profissional é inválido.");
        }

        var professional = await _professionalRepository.GetByIdAsync(
            id,
            cancellationToken);

        return professional is null
            ? null
            : ProfessionalResponse.FromEntity(professional);
    }

    public async Task<IReadOnlyList<ProfessionalResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var professionals = await _professionalRepository.GetAllAsync(
            cancellationToken);

        return professionals
            .Select(ProfessionalResponse.FromEntity)
            .ToList();
    }

    private User CreateUser(string fullName, string email, string password)
    {
        var user = new User(fullName, email, string.Empty, "Professional");
        user.SetPasswordHash(_passwordHasher.HashPassword(user, password));
        return user;
    }

    private static string ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 3)
        {
            throw new BusinessRuleException(
                "O nome do profissional deve possuir pelo menos 3 caracteres.");
        }

        return name.Trim();
    }

    private static string ValidateRegistration(string? registration)
    {
        if (string.IsNullOrWhiteSpace(registration))
        {
            throw new BusinessRuleException(
                "O registro profissional é obrigatório.");
        }

        return registration.Trim();
    }

    private static string ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new BusinessRuleException("O e-mail é obrigatório.");
        }

        try
        {
            var trimmed = email.Trim();
            var address = new MailAddress(trimmed);

            if (!address.Address.Equals(trimmed, StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException();
            }

            return trimmed.ToLowerInvariant();
        }
        catch (FormatException)
        {
            throw new BusinessRuleException("O e-mail informado é inválido.");
        }
    }

    private static void ValidatePassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            throw new BusinessRuleException(
                "A senha deve possuir pelo menos 6 caracteres.");
        }
    }

    private async Task EnsureRegistrationIsFreeAsync(
        string registrationNumber,
        int? ignoreId,
        CancellationToken cancellationToken)
    {
        var existing = await _professionalRepository.GetByRegistrationNumberAsync(
            registrationNumber,
            cancellationToken);

        if (existing is not null && existing.Id != ignoreId)
        {
            throw new BusinessRuleException(
                "Esse registro profissional já está cadastrado.");
        }
    }

    private async Task EnsureSpecialtyExistsAsync(
        int specialtyId,
        CancellationToken cancellationToken)
    {
        var specialty = await _specialtyRepository.GetByIdAsync(
            specialtyId,
            cancellationToken);

        if (specialty is null)
        {
            throw new BusinessRuleException("A especialidade informada não existe.");
        }
    }

    private async Task EnsureEmailIsFreeAsync(
        string email,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByEmailAsync(email, cancellationToken) is not null ||
            await _professionalRepository.GetByEmailAsync(email, cancellationToken) is not null)
        {
            throw new BusinessRuleException("Já existe um cadastro com este e-mail.");
        }
    }

    public async Task DeleteAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessRuleException("Profissional não encontrado.");

        if (await _professionalRepository.HasAppointmentsAsync(id, cancellationToken))
        {
            throw new BusinessRuleException(
                "Não é possível excluir: o profissional possui agendamentos. " +
                "Cancele ou remova os agendamentos antes.");
        }

        if (!string.IsNullOrEmpty(professional.Email))
        {
            var user = await _userRepository.GetByEmailAsync(
                professional.Email,
                cancellationToken);

            if (user is not null && user.Role == "Professional")
            {
                _userRepository.Remove(user);
            }
        }

        _professionalRepository.Remove(professional);

        await _professionalRepository.SaveChangesAsync(cancellationToken);
    }
}