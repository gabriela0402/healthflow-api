using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Appointments;

public sealed record AppointmentProfessionalItemResponse(
    int Id,
    DateTime ScheduledAt,
    string Status,
    string? Notes,
    int PatientId,
    string PatientName,
    DateTime? PatientDateOfBirth)
{
    public static AppointmentProfessionalItemResponse FromEntity(Appointment a) =>
        new(
            a.Id,
            a.ScheduledAt,
            a.Status.ToString(),
            a.Notes,
            a.PatientId,
            a.Patient.FullName,
            a.Patient.DateOfBirth is { Year: > 1900 } ? a.Patient.DateOfBirth : null);
}