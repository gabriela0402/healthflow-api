using HealthFlow.Model.Entities;

namespace HealthFlow.Repository.Interfaces;

public interface INotificationRepository
{
    Task<IReadOnlyList<Notification>> GetByPatientIdAsync(
        int patientId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Notification>> GetUnreadByPatientIdAsync(
        int patientId, CancellationToken cancellationToken = default);

    Task<Notification?> GetByIdAsync(
        int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Appointment>> GetAppointmentsForReminderAsync(
        DateTime from, DateTime to, CancellationToken cancellationToken = default);

    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}