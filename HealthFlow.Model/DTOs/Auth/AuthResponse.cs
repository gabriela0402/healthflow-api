namespace HealthFlow.Model.DTOs.Auth;

public record AuthResponse(
    int UserId,
    string Name,
    string Email,
    string Token);
