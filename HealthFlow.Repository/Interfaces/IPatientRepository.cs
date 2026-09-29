using HealthFlow.Model.Entities;

namespace HealthFlow.Repository.Interfaces;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Patient?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Patient>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Patient patient,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
