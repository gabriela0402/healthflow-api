using HealthFlow.Model.Entities;

namespace HealthFlow.Repository.Interfaces;

public interface IProfessionalRepository
{
    Task<IReadOnlyList<Professional>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Professional?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Professional?> GetByRegistrationNumberAsync(
        string registrationNumber,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Professional professional,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
