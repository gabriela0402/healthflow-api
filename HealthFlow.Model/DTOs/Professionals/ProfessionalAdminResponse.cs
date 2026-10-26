using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Professionals;

public record ProfessionalAdminResponse(
    int Id,
    string FullName,
    string RegistrationNumber,
    int SpecialtyId,
    string SpecialtyName,
    string? Email)
{
    public static ProfessionalAdminResponse FromEntity(Professional p) =>
        new(p.Id, p.FullName, p.RegistrationNumber,
            p.SpecialtyId, p.Specialty.Name, p.Email);
}