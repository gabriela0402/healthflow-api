namespace HealthFlow.Model.DTOs.Auth;

public record RegisterRequest(
    string Name,
    string Email,
    string Password);
