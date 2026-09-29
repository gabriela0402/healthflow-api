namespace HealthFlow.Model.DTOs.Patients;

public sealed record CreatePatientRequest(
    string FullName,
    string Email,
    string Phone,
    DateTime DateOfBirth);
