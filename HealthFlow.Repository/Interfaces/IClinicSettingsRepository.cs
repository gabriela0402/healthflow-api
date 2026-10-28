using HealthFlow.Model.Entities;

namespace HealthFlow.Repository.Interfaces;

public interface IClinicSettingsRepository
{
    Task<ClinicSettings?> GetAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ClinicSettings settings, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}