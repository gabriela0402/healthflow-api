using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Patients;

public sealed record PatientResponse(
    int Id,
    string FullName,
    string Email,
    string Phone,
    DateTime DateOfBirth,
    DateTime CreatedAt)
{
    public static PatientResponse FromEntity(Patient patient)
    {
        return new PatientResponse(
            patient.Id,
            patient.FullName,
            patient.Email,
            patient.Phone,
            patient.DateOfBirth,
            patient.CreatedAt);
    }
}
