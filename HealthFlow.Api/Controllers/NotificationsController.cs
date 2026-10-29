using System.Security.Claims;
using HealthFlow.Model.DTOs.Notifications;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize(Roles = "Patient")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service) => _service = service;

    private string? Email => User.FindFirstValue(ClaimTypes.Email);

    [HttpGet("me")]
    public async Task<ActionResult<IReadOnlyList<NotificationResponse>>> GetMine(
        CancellationToken ct)
        => Email is null ? Unauthorized() : Ok(await _service.GetMineAsync(Email, ct));

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken ct)
    {
        if (Email is null) return Unauthorized();
        await _service.MarkAsReadAsync(Email, id, ct);
        return NoContent();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken ct)
    {
        if (Email is null) return Unauthorized();
        await _service.MarkAllAsReadAsync(Email, ct);
        return NoContent();
    }

    [HttpGet("preferences/me")]
    public async Task<ActionResult<NotificationPreferencesResponse>> GetPreferences(
        CancellationToken ct)
        => Email is null ? Unauthorized() : Ok(await _service.GetPreferencesAsync(Email, ct));

    [HttpPut("preferences/me")]
    public async Task<ActionResult<NotificationPreferencesResponse>> UpdatePreferences(
        UpdateNotificationPreferencesRequest request, CancellationToken ct)
        => Email is null
            ? Unauthorized()
            : Ok(await _service.UpdatePreferencesAsync(Email, request, ct));
}