using HealthFlow.Data.Contexts;
using HealthFlow.Model.Entities;
using HealthFlow.Model.Enums;
using HealthFlow.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthFlow.Repository.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly HealthFlowDbContext _context;

    public NotificationRepository(HealthFlowDbContext context) => _context = context;

    public async Task<IReadOnlyList<Notification>> GetByPatientIdAsync(
        int patientId, CancellationToken cancellationToken = default)
        => await _context.Notifications
            .AsNoTracking()
            .Where(n => n.PatientId == patientId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Notification>> GetUnreadByPatientIdAsync(
        int patientId, CancellationToken cancellationToken = default)
        => await _context.Notifications
            .Where(n => n.PatientId == patientId && !n.IsRead)
            .ToListAsync(cancellationToken);

    public async Task<Notification?> GetByIdAsync(
        int id, CancellationToken cancellationToken = default)
        => await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Appointment>> GetAppointmentsForReminderAsync(
        DateTime from, DateTime to, CancellationToken cancellationToken = default)
        => await _context.Appointments
            .AsNoTracking()
            .Include(a => a.Patient)
            .Include(a => a.Professional)
            .Where(a => a.ScheduledAt > from && a.ScheduledAt <= to &&
                        (a.Status == AppointmentStatus.Scheduled ||
                         a.Status == AppointmentStatus.Confirmed) &&
                        !_context.Notifications.Any(n =>
                            n.AppointmentId == a.Id && n.Type == "reminder"))
            .ToListAsync(cancellationToken);

    public async Task AddAsync(
        Notification notification, CancellationToken cancellationToken = default)
        => await _context.Notifications.AddAsync(notification, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}