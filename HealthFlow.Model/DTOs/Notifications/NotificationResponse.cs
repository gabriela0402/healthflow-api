using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Notifications;

public record NotificationResponse(
    int Id, string Type, string Title, string Description,
    bool IsRead, DateTime CreatedAt)
{
    public static NotificationResponse FromEntity(Notification n) =>
        new(n.Id, n.Type, n.Title, n.Description, n.IsRead, n.CreatedAt);
}