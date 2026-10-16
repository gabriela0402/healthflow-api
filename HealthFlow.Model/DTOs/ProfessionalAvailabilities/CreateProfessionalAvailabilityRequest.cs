namespace HealthFlow.Model.DTOs.ProfessionalAvailabilities;

public record CreateProfessionalAvailabilityRequest(
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int SlotDurationInMinutes = 30);
