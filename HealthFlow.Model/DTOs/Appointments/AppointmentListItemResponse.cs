using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Appointments;

public record AppointmentListItemResponse(
    int Id,
    DateTime ScheduledAt,
    string Status,
    string? Notes,
    int ProfessionalId,
    string ProfessionalName,
    string Specialty)
{
    public static AppointmentListItemResponse FromEntity(
        Appointment appointment)
    {
        return new AppointmentListItemResponse(
            appointment.Id,
            appointment.ScheduledAt,
            appointment.Status.ToString(),
            appointment.Notes,
            appointment.ProfessionalId,
            appointment.Professional.FullName,
            appointment.Professional.Specialty.Name);
    }
}