using HealthFlow.Model.Entities;

namespace HealthFlow.Repository.Interfaces;

public interface IAppointmentRepository
{
    Task<IReadOnlyList<Appointment>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Appointment?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAtDateAsync(
        int professionalId,
        DateTime scheduledAt,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Appointment appointment,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Appointment>> GetByPatientIdAsync(
        int patientId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Appointment>> GetAllDetailedAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Appointment>> GetByProfessionalIdAsync(
        int professionalId,
        CancellationToken cancellationToken = default);
}
