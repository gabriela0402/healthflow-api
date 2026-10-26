namespace HealthFlow.Model.DTOs.Professionals;

public record UpdateProfessionalRequest(
    string FullName,
    string RegistrationNumber,
    int SpecialtyId,
    string? Email,
    string? NewPassword);