using HealthFlow.Data.Contexts;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthFlow.Repository.Repositories;

public sealed class SpecialtyRepository : ISpecialtyRepository
{
    private readonly HealthFlowDbContext _context;

    public SpecialtyRepository(HealthFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Specialty>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Specialties
            .AsNoTracking()
            .OrderBy(specialty => specialty.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Specialty?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _context.Specialties
            .FirstOrDefaultAsync(
                specialty => specialty.Name == name,
                cancellationToken);
    }

    public async Task AddAsync(
        Specialty specialty,
        CancellationToken cancellationToken = default)
    {
        await _context.Specialties.AddAsync(
            specialty,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
