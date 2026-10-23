using HealthFlow.Model.DTOs.Specialties;

namespace HealthFlow.Service.Interfaces;

public interface ISpecialtyService
{
    Task<SpecialtyResponse> CreateAsync(
        CreateSpecialtyRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SpecialtyResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<SpecialtyResponse> UpdateAsync(
        int id, UpdateSpecialtyRequest request, CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id, CancellationToken cancellationToken = default);
}
