using HealthFlow.Model.Entities;
using HealthFlow.Model.Enums;

namespace HealthFlow.Model.DTOs.Patients;

public sealed record PatientListItemResponse(
    int Id,
    string FullName,
    string Email,
    string? Phone,
    DateTime? DateOfBirth,
    string? Gender,
    DateTime CreatedAt,
    DateTime? NextAppointmentAt,
    int UpcomingCount,
    string Status)
{
    public static PatientListItemResponse FromEntity(Patient patient, DateTime now)
    {
        var upcoming = patient.Appointments
            .Where(a => a.ScheduledAt >= now &&
                        (a.Status == AppointmentStatus.Scheduled ||
                         a.Status == AppointmentStatus.Confirmed))
            .OrderBy(a => a.ScheduledAt)
            .ToList();

        var hasRecent = patient.Appointments.Any(a =>
            a.ScheduledAt < now &&
            a.ScheduledAt >= now.AddDays(-90) &&
            a.Status != AppointmentStatus.Cancelled);

        var status =
            upcoming.Any(a => a.Status == AppointmentStatus.Confirmed) || hasRecent
                ? "active"
                : upcoming.Count > 0
                    ? "pending"
                    : "inactive";

        return new PatientListItemResponse(
            patient.Id,
            patient.FullName,
            patient.Email,
            patient.Phone,
            patient.DateOfBirth is { Year: > 1900 } ? patient.DateOfBirth : null,
            patient.Gender,
            patient.CreatedAt,
            upcoming.Count > 0 ? upcoming[0].ScheduledAt : null,
            upcoming.Count,
            status);
    }
}