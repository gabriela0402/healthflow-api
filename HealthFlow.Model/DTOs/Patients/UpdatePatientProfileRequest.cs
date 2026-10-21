namespace HealthFlow.Model.DTOs.Patients;

public sealed record UpdatePatientProfileRequest(
    string FullName,
    string? Phone,
    DateTime? DateOfBirth,
    string? Gender,
    string? Cpf,
    string? Address,
    string? EmergencyContactName,
    string? EmergencyContactPhone);