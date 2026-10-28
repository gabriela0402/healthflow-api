using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Settings;

public record ClinicSettingsResponse(
    string ClinicName, string? Email, string? Phone, string? Address,
    bool AppointmentReminders, bool AutomaticConfirmation)
{
    public static ClinicSettingsResponse Default =>
        new(string.Empty, null, null, null, true, false);

    public static ClinicSettingsResponse FromEntity(ClinicSettings s) =>
        new(s.ClinicName, s.Email, s.Phone, s.Address,
            s.AppointmentReminders, s.AutomaticConfirmation);
}