using HealthFlow.Model.DTOs.Settings;
using HealthFlow.Model.DTOs.Users;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize(Roles = "Admin")]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
        => _settingsService = settingsService;

    [HttpGet("clinic")]
    public async Task<ActionResult<ClinicSettingsResponse>> Get(
        CancellationToken cancellationToken)
        => Ok(await _settingsService.GetAsync(cancellationToken));

    [HttpPut("clinic")]
    public async Task<ActionResult<ClinicSettingsResponse>> Update(
        UpdateClinicSettingsRequest request, CancellationToken cancellationToken)
        => Ok(await _settingsService.UpdateAsync(request, cancellationToken));

    [HttpGet("users")]
    public async Task<ActionResult<IReadOnlyList<UserListItemResponse>>> GetUsers(
        CancellationToken cancellationToken)
        => Ok(await _settingsService.GetUsersAsync(cancellationToken));
}