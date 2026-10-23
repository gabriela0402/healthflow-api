using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Specialties;

public record SpecialtyResponse(int Id, string Name, string? Description)
{
    public static SpecialtyResponse FromEntity(Specialty specialty) =>
        new(specialty.Id, specialty.Name, specialty.Description);
}
