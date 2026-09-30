using HealthFlow.Model.DTOs.Specialties;

namespace HealthFlow.Service.Interfaces;

public interface ISpecialtyService
{
    Task<SpecialtyResponse> CreateAsync(
        CreateSpecialtyRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SpecialtyResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
