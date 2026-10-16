namespace HealthFlow.Model.Entities;

public class ProfessionalAvailability
{
    public int Id { get; private set; }

    public int ProfessionalId { get; private set; }

    public Professional Professional { get; private set; } = null!;

    public DayOfWeek DayOfWeek { get; private set; }

    public TimeSpan StartTime { get; private set; }

    public TimeSpan EndTime { get; private set; }

    public int SlotDurationInMinutes { get; private set; }

    protected ProfessionalAvailability()
    {
    }

    public ProfessionalAvailability(
        int professionalId,
        DayOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        int slotDurationInMinutes = 30)
    {
        ProfessionalId = professionalId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        SlotDurationInMinutes = slotDurationInMinutes;
    }
}
