namespace HealthFlow.Model.DTOs.Auth;

public record AuthResponse(
    int UserId,
    int? PatientId,
    string Name,
    string Email,
    string Role,
    string Token);
