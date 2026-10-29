namespace HealthFlow.Model.Entities;

public class Notification
{
    public int Id { get; private set; }
    public int PatientId { get; private set; }
    public Patient Patient { get; private set; } = null!;
    public int? AppointmentId { get; private set; }
    public string Type { get; private set; } = "system"; // appointment | reminder | system
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Notification() { }

    public Notification(int patientId, string type, string title,
        string description, int? appointmentId = null)
    {
        PatientId = patientId;
        Type = type;
        Title = title;
        Description = description;
        AppointmentId = appointmentId;
        CreatedAt = DateTime.Now;
    }

    public void MarkAsRead() => IsRead = true;
}