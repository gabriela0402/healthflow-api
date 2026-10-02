using HealthFlow.Model.DTOs.Specialties;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SpecialtiesController : ControllerBase
{
    private readonly ISpecialtyService _specialtyService;

    public SpecialtiesController(
        ISpecialtyService specialtyService)
    {
        _specialtyService = specialtyService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SpecialtyResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var specialties = await _specialtyService.GetAllAsync(
            cancellationToken);

        return Ok(specialties);
    }

    [HttpPost]
    public async Task<ActionResult<SpecialtyResponse>> Create(
        CreateSpecialtyRequest request,
        CancellationToken cancellationToken)
    {
        var specialty = await _specialtyService.CreateAsync(
            request,
            cancellationToken);

        return Created(string.Empty, specialty);
    }
}
