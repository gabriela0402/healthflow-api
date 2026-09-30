using HealthFlow.Model.DTOs.Appointments;

namespace HealthFlow.Service.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponse> CreateAsync(
        CreateAppointmentRequest request,
        CancellationToken cancellationToken = default);

    Task<AppointmentResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AppointmentResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<AppointmentResponse> ConfirmAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<AppointmentResponse> CancelAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<AppointmentResponse> CompleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
