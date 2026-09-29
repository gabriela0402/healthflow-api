using HealthFlow.Model.DTOs.Patients;

namespace HealthFlow.Service.Interfaces;

public interface IPatientService
{
    Task<PatientResponse> CreateAsync(
        CreatePatientRequest request,
        CancellationToken cancellationToken = default);

    Task<PatientResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PatientResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
