using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Specialties;

public sealed record SpecialtyResponse(
    int Id,
    string Name)
{
    public static SpecialtyResponse FromEntity(Specialty specialty)
    {
        return new SpecialtyResponse(
            specialty.Id,
            specialty.Name);
    }
}
