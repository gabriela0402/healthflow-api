using HealthFlow.Model.DTOs.Notifications;
using HealthFlow.Model.Entities;

namespace HealthFlow.Service.Interfaces;

public interface INotificationService
{
    Task NotifyAppointmentAsync(
        Patient patient, string title, string description,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NotificationResponse>> GetMineAsync(
        string email, CancellationToken cancellationToken = default);

    Task MarkAsReadAsync(string email, int id, CancellationToken cancellationToken = default);

    Task MarkAllAsReadAsync(string email, CancellationToken cancellationToken = default);

    Task<NotificationPreferencesResponse> GetPreferencesAsync(
        string email, CancellationToken cancellationToken = default);

    Task<NotificationPreferencesResponse> UpdatePreferencesAsync(
        string email, UpdateNotificationPreferencesRequest request,
        CancellationToken cancellationToken = default);

    Task SendRemindersAsync(CancellationToken cancellationToken = default);
}