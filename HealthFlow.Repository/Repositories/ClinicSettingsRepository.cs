using HealthFlow.Data.Contexts;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthFlow.Repository.Repositories;

public class ClinicSettingsRepository : IClinicSettingsRepository
{
    private readonly HealthFlowDbContext _context;

    public ClinicSettingsRepository(HealthFlowDbContext context) => _context = context;

    public async Task<ClinicSettings?> GetAsync(CancellationToken cancellationToken = default)
        => await _context.Settings.OrderBy(s => s.Id).FirstOrDefaultAsync(cancellationToken);

    public async Task AddAsync(ClinicSettings settings, CancellationToken cancellationToken = default)
        => await _context.Settings.AddAsync(settings, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}