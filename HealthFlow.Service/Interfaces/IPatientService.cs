using HealthFlow.Model.DTOs.Patients;

namespace HealthFlow.Service.Interfaces;

public interface IPatientService
{
    Task<PatientResponse> CreateAsync(
        CreatePatientRequest request,
        CancellationToken cancellationToken = default);

    Task<PatientResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PatientResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
