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
}
