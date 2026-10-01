namespace HealthFlow.Model.DTOs.Auth;

public record LoginRequest(
    string Email,
    string Password);
