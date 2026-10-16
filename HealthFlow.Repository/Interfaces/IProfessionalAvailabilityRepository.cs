using HealthFlow.Model.Entities;

namespace HealthFlow.Repository.Interfaces;

public interface IProfessionalAvailabilityRepository
{
    Task AddAsync(
        ProfessionalAvailability availability,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfessionalAvailability>>
        GetByProfessionalIdAsync(
            int professionalId,
            CancellationToken cancellationToken = default);

    Task<bool> ExistsForProfessionalAndDayAsync(
        int professionalId,
        DayOfWeek dayOfWeek,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
