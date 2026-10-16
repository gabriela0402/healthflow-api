using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.ProfessionalAvailabilities;

public record ProfessionalAvailabilityResponse(
    int Id,
    int ProfessionalId,
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int SlotDurationInMinutes)
{
    public static ProfessionalAvailabilityResponse
        FromEntity(
            ProfessionalAvailability availability)
    {
        return new ProfessionalAvailabilityResponse(
            availability.Id,
            availability.ProfessionalId,
            availability.DayOfWeek,
            availability.StartTime,
            availability.EndTime,
            availability.SlotDurationInMinutes);
    }
}
