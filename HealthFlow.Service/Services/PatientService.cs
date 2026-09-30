using System.Net.Mail;
using HealthFlow.Model.DTOs.Patients;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;

namespace HealthFlow.Service.Services;

public sealed class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<PatientResponse> CreateAsync(
        CreatePatientRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateCreateRequest(request);

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var existingPatient =
            await _patientRepository.GetByEmailAsync(
                normalizedEmail,
                cancellationToken);

        if (existingPatient is not null)
        {
            throw new BusinessRuleException(
                "Já existe um paciente cadastrado com este e-mail.");
        }

        var patient = new Patient(
            request.FullName.Trim(),
            normalizedEmail,
            request.Phone.Trim(),
            request.DateOfBirth.Date);

        await _patientRepository.AddAsync(
            patient,
            cancellationToken);

        await _patientRepository.SaveChangesAsync(
            cancellationToken);

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

    private static void ValidateCreateRequest(
        CreatePatientRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new BusinessRuleException(
                "O nome completo é obrigatório.");
        }

        if (request.FullName.Trim().Length < 3)
        {
            throw new BusinessRuleException(
                "O nome completo deve possuir pelo menos 3 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new BusinessRuleException(
                "O e-mail é obrigatório.");
        }

        if (!IsValidEmail(request.Email))
        {
            throw new BusinessRuleException(
                "O e-mail informado é inválido.");
        }

        if (string.IsNullOrWhiteSpace(request.Phone))
        {
            throw new BusinessRuleException(
                "O telefone é obrigatório.");
        }

        if (request.DateOfBirth.Date > DateTime.UtcNow.Date)
        {
            throw new BusinessRuleException(
                "A data de nascimento não pode estar no futuro.");
        }
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
}
