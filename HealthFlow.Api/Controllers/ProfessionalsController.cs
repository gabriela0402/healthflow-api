using HealthFlow.Model.DTOs.ProfessionalAvailabilities;
using HealthFlow.Model.DTOs.Professionals;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}
