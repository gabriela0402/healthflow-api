namespace HealthFlow.Model.DTOs.Settings;

public record UpdateClinicSettingsRequest(
    string ClinicName, string? Email, string? Phone, string? Address,
    bool AppointmentReminders, bool AutomaticConfirmation);