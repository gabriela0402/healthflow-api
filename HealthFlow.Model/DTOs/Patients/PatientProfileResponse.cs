using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Patients;

public sealed record PatientProfileResponse(
    int Id,
    string FullName,
    string Email,
    string? Phone,
    DateTime? DateOfBirth,
    string? Gender,
    string? Cpf,
    string? Address,
    string? EmergencyContactName,
    string? EmergencyContactPhone)
{
    public static PatientProfileResponse FromEntity(Patient patient)
    {
        return new PatientProfileResponse(
            patient.Id,
            patient.FullName,
            patient.Email,
            patient.Phone,
            patient.DateOfBirth is { Year: > 1900 } ? patient.DateOfBirth : null,
            patient.Gender,
            patient.Cpf,
            patient.Address,
            patient.EmergencyContactName,
            patient.EmergencyContactPhone);
    }
}