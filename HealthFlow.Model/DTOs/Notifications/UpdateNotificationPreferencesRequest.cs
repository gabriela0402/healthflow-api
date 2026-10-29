namespace HealthFlow.Model.DTOs.Notifications;

public record UpdateNotificationPreferencesRequest(
    bool Email, bool Reminders, bool AppointmentUpdates);