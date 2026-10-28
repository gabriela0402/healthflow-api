using HealthFlow.Model.DTOs.Settings;
using HealthFlow.Model.DTOs.Users;

namespace HealthFlow.Service.Interfaces;

public interface ISettingsService
{
    Task<ClinicSettingsResponse> GetAsync(CancellationToken cancellationToken = default);
    Task<ClinicSettingsResponse> UpdateAsync(
        UpdateClinicSettingsRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserListItemResponse>> GetUsersAsync(
        CancellationToken cancellationToken = default);
}