using System.Security.Claims;
using HealthFlow.Model.DTOs.Notifications;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/professional-notifications")]
[Authorize(Roles = "Professional")]
public class ProfessionalNotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public ProfessionalNotificationsController(INotificationService service) => _service = service;

    private string? Email => User.FindFirstValue(ClaimTypes.Email);

    [HttpGet("me")]
    public async Task<ActionResult<IReadOnlyList<NotificationResponse>>> GetMine(
        CancellationToken ct)
        => Email is null ? Unauthorized() : Ok(await _service.GetMineAsProfessionalAsync(Email, ct));

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken ct)
    {
        if (Email is null) return Unauthorized();
        await _service.MarkAsReadAsProfessionalAsync(Email, id, ct);
        return NoContent();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken ct)
    {
        if (Email is null) return Unauthorized();
        await _service.MarkAllAsReadAsProfessionalAsync(Email, ct);
        return NoContent();
    }
}