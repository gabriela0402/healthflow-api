namespace HealthFlow.Model.DTOs.Patients;

public sealed record CreatePatientRequest(
    string FullName,
    string Email,
    string Password,
    string? Phone,
    DateTime? DateOfBirth,
    string? Gender,
    string? Cpf,
    string? Address);