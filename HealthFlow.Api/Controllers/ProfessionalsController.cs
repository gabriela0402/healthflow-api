using HealthFlow.Model.DTOs.Professionals;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfessionalsController : ControllerBase
{
    private readonly IProfessionalService _professionalService;

    public ProfessionalsController(
        IProfessionalService professionalService)
    {
        _professionalService = professionalService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProfessionalResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var professionals = await _professionalService.GetAllAsync(
            cancellationToken);

        return Ok(professionals);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProfessionalResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var professional = await _professionalService.GetByIdAsync(
            id,
            cancellationToken);

        if (professional is null)
        {
            return NotFound("Profissional não encontrado.");
        }

        return Ok(professional);
    }

    [HttpPost]
    public async Task<ActionResult<ProfessionalResponse>> Create(
        CreateProfessionalRequest request,
        CancellationToken cancellationToken)
    {
        var professional = await _professionalService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = professional.Id },
            professional);
    }
}
