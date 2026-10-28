using HealthFlow.Model.DTOs.Professionals;

namespace HealthFlow.Service.Interfaces;

public interface IProfessionalService
{
    Task<ProfessionalResponse> CreateAsync(
        CreateProfessionalRequest request,
        CancellationToken cancellationToken = default);

    Task<ProfessionalResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfessionalResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ProfessionalResponse> UpdateAsync(
        int id,
        UpdateProfessionalRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id, 
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfessionalAdminResponse>> GetOverviewAsync(
        CancellationToken cancellationToken = default);

    Task<ProfessionalProfileResponse> GetMyProfileAsync(
        string email, CancellationToken cancellationToken = default);

    Task<ProfessionalProfileResponse> UpdateMyProfileAsync(
        string email,
        UpdateProfessionalProfileRequest request,
        CancellationToken cancellationToken = default);
}
