using HealthFlow.Model.Entities;

namespace HealthFlow.Repository.Interfaces;

public interface ISpecialtyRepository
{
    Task<IReadOnlyList<Specialty>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Specialty?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Specialty specialty,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<Specialty?> GetByIdAsync(
        int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(
        string name, int? excludeId, CancellationToken cancellationToken = default);
    Task<bool> HasProfessionalsAsync(
        int id, CancellationToken cancellationToken = default);
    void Remove(
        Specialty specialty);
}
