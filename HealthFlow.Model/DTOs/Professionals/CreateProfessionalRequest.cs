namespace HealthFlow.Model.DTOs.Professionals;

public record CreateProfessionalRequest(
    string FullName,
    string RegistrationNumber,
    int SpecialtyId);
