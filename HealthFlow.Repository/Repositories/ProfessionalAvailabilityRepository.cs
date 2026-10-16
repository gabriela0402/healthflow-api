using HealthFlow.Data.Contexts;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthFlow.Repository.Repositories;

public class ProfessionalAvailabilityRepository
    : IProfessionalAvailabilityRepository
{
    private readonly HealthFlowDbContext _context;

    public ProfessionalAvailabilityRepository(
        HealthFlowDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ProfessionalAvailability availability,
        CancellationToken cancellationToken = default)
    {
        await _context.ProfessionalAvailabilities.AddAsync(
            availability,
            cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalAvailability>>
        GetByProfessionalIdAsync(
            int professionalId,
            CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalAvailabilities
            .AsNoTracking()
            .Where(availability =>
                availability.ProfessionalId == professionalId)
            .OrderBy(availability => availability.DayOfWeek)
            .ThenBy(availability => availability.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsForProfessionalAndDayAsync(
        int professionalId,
        DayOfWeek dayOfWeek,
        CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalAvailabilities
            .AnyAsync(
                availability =>
                    availability.ProfessionalId ==
                        professionalId &&
                    availability.DayOfWeek ==
                        dayOfWeek,
                cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
