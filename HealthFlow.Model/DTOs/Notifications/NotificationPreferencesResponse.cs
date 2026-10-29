namespace HealthFlow.Model.DTOs.Notifications;

public record NotificationPreferencesResponse(
    bool Email, bool Reminders, bool AppointmentUpdates);