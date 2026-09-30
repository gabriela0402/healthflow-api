using HealthFlow.Data.Contexts;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using HealthFlow.Model.Enums;


namespace HealthFlow.Repository.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly HealthFlowDbContext _context;

    public AppointmentRepository(HealthFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .AsNoTracking()
            .Include(appointment => appointment.Patient)
            .Include(appointment => appointment.Professional)
            .OrderBy(appointment => appointment.ScheduledAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Include(appointment => appointment.Patient)
            .Include(appointment => appointment.Professional)
            .FirstOrDefaultAsync(
                appointment => appointment.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        Appointment appointment,
        CancellationToken cancellationToken = default)
    {
        await _context.Appointments.AddAsync(
            appointment,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> ExistsAtDateAsync(
        int professionalId,
        DateTime scheduledAt,
        CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .AnyAsync(
                appointment =>
                    appointment.ProfessionalId == professionalId &&
                    appointment.ScheduledAt == scheduledAt &&
                    appointment.Status != AppointmentStatus.Cancelled,
                cancellationToken);
    }

}
