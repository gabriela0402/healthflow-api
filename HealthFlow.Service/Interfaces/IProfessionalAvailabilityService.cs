using HealthFlow.Model.DTOs.ProfessionalAvailabilities;

namespace HealthFlow.Service.Interfaces;

public interface IProfessionalAvailabilityService
{
    Task<ProfessionalAvailabilityResponse> CreateAsync(
        int professionalId,
        CreateProfessionalAvailabilityRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfessionalAvailabilityResponse>>
        GetByProfessionalIdAsync(
            int professionalId,
            CancellationToken cancellationToken = default);
}
