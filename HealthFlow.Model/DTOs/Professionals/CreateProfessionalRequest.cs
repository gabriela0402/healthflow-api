namespace HealthFlow.Model.DTOs.Professionals;

public record CreateProfessionalRequest(
    string FullName,
    string Email,
    string Password,
    string RegistrationNumber,
    int SpecialtyId);