using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Appointments;

public sealed record AppointmentAdminItemResponse(
    int Id,
    DateTime ScheduledAt,
    string Status,
    string? Notes,
    int PatientId,
    string PatientName,
    DateTime? PatientDateOfBirth,
    int ProfessionalId,
    string ProfessionalName,
    string Specialty)
{
    public static AppointmentAdminItemResponse FromEntity(Appointment a)
    {
        return new AppointmentAdminItemResponse(
            a.Id,
            a.ScheduledAt,
            a.Status.ToString(),
            a.Notes,
            a.PatientId,
            a.Patient.FullName,
            a.Patient.DateOfBirth is { Year: > 1900 } ? a.Patient.DateOfBirth : null,
            a.ProfessionalId,
            a.Professional.FullName,
            a.Professional.Specialty.Name);
    }
}