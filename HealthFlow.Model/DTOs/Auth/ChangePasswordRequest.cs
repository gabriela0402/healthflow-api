namespace HealthFlow.Model.DTOs.Auth;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);