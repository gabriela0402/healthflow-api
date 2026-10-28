using HealthFlow.Model.DTOs.Settings;
using HealthFlow.Model.DTOs.Users;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;

namespace HealthFlow.Service.Services;

public class SettingsService : ISettingsService
{
    private readonly IClinicSettingsRepository _settingsRepository;
    private readonly IUserRepository _userRepository;

    public SettingsService(
        IClinicSettingsRepository settingsRepository,
        IUserRepository userRepository)
    {
        _settingsRepository = settingsRepository;
        _userRepository = userRepository;
    }

    public async Task<ClinicSettingsResponse> GetAsync(
        CancellationToken cancellationToken = default)
    {
        var settings = await _settingsRepository.GetAsync(cancellationToken);

        return settings is null
            ? ClinicSettingsResponse.Default
            : ClinicSettingsResponse.FromEntity(settings);
    }

    public async Task<ClinicSettingsResponse> UpdateAsync(
        UpdateClinicSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.ClinicName) ||
            request.ClinicName.Trim().Length < 3)
        {
            throw new BusinessRuleException(
                "O nome da clínica deve possuir pelo menos 3 caracteres.");
        }

        var email = Clean(request.Email);

        if (email is not null && !email.Contains('@'))
        {
            throw new BusinessRuleException("O e-mail informado é inválido.");
        }

        var settings = await _settingsRepository.GetAsync(cancellationToken);

        if (settings is null)
        {
            settings = new ClinicSettings(request.ClinicName.Trim());
            await _settingsRepository.AddAsync(settings, cancellationToken);
        }

        settings.Update(
            request.ClinicName.Trim(), email, Clean(request.Phone), Clean(request.Address),
            request.AppointmentReminders, request.AutomaticConfirmation);

        await _settingsRepository.SaveChangesAsync(cancellationToken);

        return ClinicSettingsResponse.FromEntity(settings);
    }

    public async Task<IReadOnlyList<UserListItemResponse>> GetUsersAsync(
        CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(UserListItemResponse.FromEntity).ToList();
    }

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}