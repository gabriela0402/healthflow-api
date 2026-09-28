using HealthFlow.Model.Enums;

namespace HealthFlow.Model.Entities;

public class Appointment
{
    public Guid Id { get; private set; }

    public Guid PatientId { get; private set; }

    public Patient Patient { get; private set; } = null!;

    public Guid ProfessionalId { get; private set; }

    public Professional Professional { get; private set; } = null!;

    public DateTime ScheduledAt { get; private set; }

    public AppointmentStatus Status { get; private set; }

    public string? Notes { get; private set; }

    public DateTime CreatedAt { get; private set; }

    protected Appointment()
    {
    }

    public Appointment(
        Guid patientId,
        Guid professionalId,
        DateTime scheduledAt,
        string? notes = null)
    {
        Id = Guid.NewGuid();
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
