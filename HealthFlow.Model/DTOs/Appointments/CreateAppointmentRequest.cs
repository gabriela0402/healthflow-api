namespace HealthFlow.Model.DTOs.Appointments;

public record CreateAppointmentRequest(
    int PatientId,
    int ProfessionalId,
    DateTime ScheduledAt,
    string? Notes);
