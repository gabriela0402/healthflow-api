using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Users;

public record UserListItemResponse(
    int Id, string Name, string Email, string Role, DateTime CreatedAt)
{
    public static UserListItemResponse FromEntity(User u) =>
        new(u.Id, u.Name, u.Email, u.Role, u.CreatedAt);
}