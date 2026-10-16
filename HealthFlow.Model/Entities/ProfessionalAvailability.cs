using HealthFlow.Model.Entities;

public class ProfessionalAvailability
{
    public int Id { get; private set; }

    public int ProfessionalId { get; private set; }

    public DayOfWeek DayOfWeek { get; private set; }

    public TimeOnly StartTime { get; private set; }

    public TimeOnly EndTime { get; private set; }

    public int SlotDurationInMinutes { get; private set; }

    public Professional Professional { get; private set; } = null!;
}
