using HealthFlow.Data.Contexts;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthFlow.Repository.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly HealthFlowDbContext _context;

    public PatientRepository(HealthFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(
                patient => patient.Id == id,
                cancellationToken);
    }

    public async Task<Patient?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(
                patient => patient.Email == email,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Patient>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .AsNoTracking()
            .OrderBy(patient => patient.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Patient patient,
        CancellationToken cancellationToken = default)
    {
        await _context.Patients.AddAsync(patient, cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
