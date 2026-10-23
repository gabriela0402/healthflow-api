using HealthFlow.Model.DTOs.Patients;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace HealthFlow.Service.Services;

public sealed class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher = new();


    public PatientService(
        IPatientRepository patientRepository,
        IUserRepository userRepository)
    {
        _patientRepository = patientRepository;
        _userRepository = userRepository;
    }

    public async Task<PatientResponse> CreateAsync(
    CreatePatientRequest request,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            request.FullName.Trim().Length < 3)
        {
            throw new BusinessRuleException(
                "O nome completo deve possuir pelo menos 3 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
        {
            throw new BusinessRuleException("O e-mail informado é inválido.");
        }

        if (string.IsNullOrWhiteSpace(request.Password) ||
            request.Password.Length < 6)
        {
            throw new BusinessRuleException(
                "A senha inicial deve possuir pelo menos 6 caracteres.");
        }

        DateTime? dateOfBirth = null;

        if (request.DateOfBirth.HasValue)
        {
            var birth = request.DateOfBirth.Value.Date;

            if (birth > DateTime.UtcNow.Date || birth.Year <= 1900)
            {
                throw new BusinessRuleException(
                    "A data de nascimento é inválida.");
            }

            dateOfBirth = birth;
        }

        string? cpf = null;

        if (!string.IsNullOrWhiteSpace(request.Cpf))
        {
            cpf = new string(request.Cpf.Where(char.IsDigit).ToArray());

            if (!IsValidCpf(cpf))
            {
                throw new BusinessRuleException("O CPF informado é inválido.");
            }
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var fullName = request.FullName.Trim();

        if (await _patientRepository.GetByEmailAsync(email, cancellationToken) is not null ||
            await _userRepository.GetByEmailAsync(email, cancellationToken) is not null)
        {
            throw new BusinessRuleException(
                "Já existe um cadastro com este e-mail.");
        }

        var user = new User(fullName, email, string.Empty, "Patient");
        user.SetPasswordHash(_passwordHasher.HashPassword(user, request.Password));

        var patient = new Patient(fullName, email);
        patient.UpdateProfile(
            fullName,
            NullIfEmpty(request.Phone),
            dateOfBirth,
            NullIfEmpty(request.Gender),
            cpf,
            NullIfEmpty(request.Address),
            null,
            null);

        await _userRepository.AddAsync(user, cancellationToken);
        await _patientRepository.AddAsync(patient, cancellationToken);

        await _patientRepository.SaveChangesAsync(cancellationToken);

        return PatientResponse.FromEntity(patient);
    }

    public async Task<PatientResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
           throw new BusinessRuleException(
             "O identificador do paciente é inválido.");
        }


        var patient = await _patientRepository.GetByIdAsync(
            id,
            cancellationToken);

        return patient is null
            ? null
            : PatientResponse.FromEntity(patient);
    }

    public async Task<IReadOnlyList<PatientResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllAsync(
            cancellationToken);

        return patients
            .Select(PatientResponse.FromEntity)
            .ToList();
    }


    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email.Trim());

            return mailAddress.Address.Equals(
                email.Trim(),
                StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public async Task<PatientProfileResponse> GetMyProfileAsync(
    string email,
    CancellationToken cancellationToken = default)
    {
        var patient = await GetPatientByEmailOrThrowAsync(email, cancellationToken);

        return PatientProfileResponse.FromEntity(patient);
    }

    public async Task<PatientProfileResponse> UpdateMyProfileAsync(
        string email,
        UpdatePatientProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            request.FullName.Trim().Length < 3)
        {
            throw new BusinessRuleException(
                "O nome completo deve possuir pelo menos 3 caracteres.");
        }

        DateTime? dateOfBirth = null;

        if (request.DateOfBirth.HasValue)
        {
            dateOfBirth = request.DateOfBirth.Value.Date;

            if (dateOfBirth > DateTime.UtcNow.Date)
            {
                throw new BusinessRuleException(
                    "A data de nascimento não pode estar no futuro.");
            }

            if (dateOfBirth.Value.Year <= 1900)
            {
                throw new BusinessRuleException(
                    "A data de nascimento é inválida.");
            }
        }

        string? cpf = null;

        if (!string.IsNullOrWhiteSpace(request.Cpf))
        {
            cpf = new string(request.Cpf.Where(char.IsDigit).ToArray());

            if (!IsValidCpf(cpf))
            {
                throw new BusinessRuleException("O CPF informado é inválido.");
            }
        }

        var patient = await GetPatientByEmailOrThrowAsync(email, cancellationToken);

        var fullName = request.FullName.Trim();

        patient.UpdateProfile(
            fullName,
            NullIfEmpty(request.Phone),
            dateOfBirth,
            NullIfEmpty(request.Gender),
            cpf,
            NullIfEmpty(request.Address),
            NullIfEmpty(request.EmergencyContactName),
            NullIfEmpty(request.EmergencyContactPhone));

        var user = await _userRepository.GetByEmailAsync(
            patient.Email,
            cancellationToken);

        user?.SetName(fullName);

        await _patientRepository.SaveChangesAsync(cancellationToken);

        return PatientProfileResponse.FromEntity(patient);
    }

    private async Task<Patient> GetPatientByEmailOrThrowAsync(
        string email,
        CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByEmailAsync(
            email.Trim().ToLowerInvariant(),
            cancellationToken);

        if (patient is null)
        {
            throw new BusinessRuleException(
                "Paciente não encontrado para o usuário logado.");
        }

        return patient;
    }

    private static string? NullIfEmpty(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static bool IsValidCpf(string digits)
    {
        if (digits.Length != 11 || digits.Distinct().Count() == 1)
        {
            return false;
        }

        static int CheckDigit(string d, int length)
        {
            var sum = 0;

            for (var i = 0; i < length; i++)
            {
                sum += (d[i] - '0') * (length + 1 - i);
            }

            var result = sum * 10 % 11;

            return result == 10 ? 0 : result;
        }

        return CheckDigit(digits, 9) == digits[9] - '0' &&
               CheckDigit(digits, 10) == digits[10] - '0';
    }

    public async Task<IReadOnlyList<PatientListItemResponse>> GetOverviewAsync(
    CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;

        var patients = await _patientRepository.GetAllWithAppointmentsAsync(
            cancellationToken);

        return patients
            .Select(patient => PatientListItemResponse.FromEntity(patient, now))
            .ToList();
    }


}
