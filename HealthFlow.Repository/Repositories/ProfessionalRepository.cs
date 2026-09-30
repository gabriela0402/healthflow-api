using HealthFlow.Data.Contexts;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthFlow.Repository.Repositories;

public class ProfessionalRepository : IProfessionalRepository
{
    private readonly HealthFlowDbContext _context;

    public ProfessionalRepository(HealthFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Professional>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .AsNoTracking()
            .Include(professional => professional.Specialty)
            .OrderBy(professional => professional.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Professional?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(professional => professional.Specialty)
            .FirstOrDefaultAsync(
                professional => professional.Id == id,
                cancellationToken);
    }

    public async Task<Professional?> GetByRegistrationNumberAsync(
        string registrationNumber,
        CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .FirstOrDefaultAsync(
                professional =>
                    professional.RegistrationNumber ==
                    registrationNumber,
                cancellationToken);
    }

    public async Task AddAsync(
        Professional professional,
        CancellationToken cancellationToken = default)
    {
        await _context.Professionals.AddAsync(
            professional,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
