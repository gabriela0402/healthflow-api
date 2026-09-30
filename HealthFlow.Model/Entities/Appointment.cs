using HealthFlow.Model.Enums;

namespace HealthFlow.Model.Entities;

public class Appointment
{
    public int Id { get; private set; }

    public int PatientId { get; private set; }

    public Patient Patient { get; private set; } = null!;

    public int ProfessionalId { get; private set; }

    public Professional Professional { get; private set; } = null!;

    public DateTime ScheduledAt { get; private set; }

    public AppointmentStatus Status { get; private set; }

    public string? Notes { get; private set; }

    public DateTime CreatedAt { get; private set; }

    protected Appointment()
    {
    }

    public Appointment(
        int patientId,
        int professionalId,
        DateTime scheduledAt,
        string? notes = null)
    {
        PatientId = patientId;
        ProfessionalId = professionalId;
        ScheduledAt = scheduledAt;
        Notes = notes;
        Status = AppointmentStatus.Scheduled;
        CreatedAt = DateTime.UtcNow;
    }

    public void Confirm()
    {
        Status = AppointmentStatus.Confirmed;
    }

    public void Cancel()
    {
        Status = AppointmentStatus.Cancelled;
    }

    public void Complete()
    {
        Status = AppointmentStatus.Completed;
    }
}
