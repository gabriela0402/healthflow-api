using HealthFlow.Model.DTOs.ProfessionalAvailabilities;
using HealthFlow.Model.DTOs.Professionals;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfessionalsController : ControllerBase
{
    private readonly IProfessionalService
        _professionalService;

    private readonly IProfessionalAvailabilityService
        _availabilityService;

    public ProfessionalsController(
        IProfessionalService professionalService,
        IProfessionalAvailabilityService availabilityService)
    {
        _professionalService = professionalService;
        _availabilityService = availabilityService;
    }

    [HttpGet]
    public async Task<
        ActionResult<IReadOnlyList<ProfessionalResponse>>>
        GetAll(
            CancellationToken cancellationToken)
    {
        var professionals =
            await _professionalService.GetAllAsync(
                cancellationToken);

        return Ok(professionals);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProfessionalResponse>>
        GetById(
            int id,
            CancellationToken cancellationToken)
    {
        var professional =
            await _professionalService.GetByIdAsync(
                id,
                cancellationToken);

        if (professional is null)
        {
            return NotFound(
                "Profissional não encontrado.");
        }

        return Ok(professional);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProfessionalResponse>>
        Create(
            CreateProfessionalRequest request,
            CancellationToken cancellationToken)
    {
        var professional =
            await _professionalService.CreateAsync(
                request,
                cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = professional.Id },
            professional);
    }

    [HttpPost("{professionalId:int}/availability")]
    [Authorize(Roles = "Admin")]
    public async Task<
        ActionResult<ProfessionalAvailabilityResponse>>
        CreateAvailability(
            int professionalId,
            CreateProfessionalAvailabilityRequest request,
            CancellationToken cancellationToken)
    {
        var availability =
            await _availabilityService.CreateAsync(
                professionalId,
                request,
                cancellationToken);

        return Ok(availability);
    }

    [HttpGet("{professionalId:int}/availability")]
    public async Task<
        ActionResult<
            IReadOnlyList<ProfessionalAvailabilityResponse>>>
        GetAvailabilities(
            int professionalId,
            CancellationToken cancellationToken)
    {
        var availabilities =
            await _availabilityService
                .GetByProfessionalIdAsync(
                    professionalId,
                    cancellationToken);

        return Ok(availabilities);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProfessionalResponse>> Update(
    int id,
    UpdateProfessionalRequest request,
    CancellationToken cancellationToken)
    {
        return Ok(await _professionalService.UpdateAsync(id, request, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
    int id,
    CancellationToken cancellationToken)
    {
        await _professionalService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("overview")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IReadOnlyList<ProfessionalAdminResponse>>> GetOverview(
    CancellationToken cancellationToken)
    {
        return Ok(await _professionalService.GetOverviewAsync(cancellationToken));
    }

    [HttpGet("me/availability")]
    [Authorize(Roles = "Professional")]
    public async Task<ActionResult<IReadOnlyList<ProfessionalAvailabilityResponse>>> GetMyAvailability(
    CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email)) return Unauthorized();

        return Ok(await _availabilityService.GetMineAsync(email, cancellationToken));
    }

    [HttpPost("me/availability")]
    [Authorize(Roles = "Professional")]
    public async Task<ActionResult<ProfessionalAvailabilityResponse>> CreateMyAvailability(
        CreateProfessionalAvailabilityRequest request,
        CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email)) return Unauthorized();

        return Ok(await _availabilityService.CreateMineAsync(email, request, cancellationToken));
    }
}
