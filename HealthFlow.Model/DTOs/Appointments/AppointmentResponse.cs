using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Appointments;

public record AppointmentResponse(
    int Id,
    int PatientId,
    int ProfessionalId,
    DateTime ScheduledAt,
    string Status,
    string? Notes)
{
    public static AppointmentResponse FromEntity(
        Appointment appointment)
    {
        return new AppointmentResponse(
            appointment.Id,
            appointment.PatientId,
            appointment.ProfessionalId,
            appointment.ScheduledAt,
            appointment.Status.ToString(),
            appointment.Notes);
    }
}
