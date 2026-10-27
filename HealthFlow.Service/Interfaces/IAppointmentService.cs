using HealthFlow.Model.DTOs.Appointments;
using HealthFlow.Model.DTOs.Patients;

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

    Task<IReadOnlyList<AppointmentListItemResponse>> GetMyAppointmentsAsync(
        string email,
        CancellationToken cancellationToken = default);
    Task<int> GetPatientIdByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AppointmentAdminItemResponse>> GetOverviewAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProfessionalPatientResponse>> GetMyPatientsAsync(
        string email,
        CancellationToken cancellationToken = default);
}
