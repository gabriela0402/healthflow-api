using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Professionals;

public record ProfessionalProfileResponse(
    int Id,
    string FullName,
    string? Email,
    string? Phone,
    string RegistrationNumber,
    string SpecialtyName)
{
    public static ProfessionalProfileResponse FromEntity(Professional p) =>
        new(p.Id, p.FullName, p.Email, p.Phone,
            p.RegistrationNumber, p.Specialty.Name);
}