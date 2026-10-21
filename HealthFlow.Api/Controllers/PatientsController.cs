using HealthFlow.Model.DTOs.Patients;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatientResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var patients = await _patientService.GetAllAsync(
            cancellationToken);

        return Ok(patients);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var patient = await _patientService.GetByIdAsync(
            id,
            cancellationToken);

        if (patient is null)
        {
            return NotFound("Paciente não encontrado.");
        }

        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponse>> Create(
        CreatePatientRequest request,
        CancellationToken cancellationToken)
    {
        var patient = await _patientService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = patient.Id },
            patient);
    }

    [HttpGet("me")]
    public async Task<ActionResult<PatientProfileResponse>> GetMine(
    CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            return Unauthorized();
        }

        return Ok(await _patientService.GetMyProfileAsync(email, cancellationToken));
    }

    [HttpPut("me")]
    public async Task<ActionResult<PatientProfileResponse>> UpdateMine(
        UpdatePatientProfileRequest request,
        CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            return Unauthorized();
        }

        return Ok(await _patientService.UpdateMyProfileAsync(
            email,
            request,
            cancellationToken));
    }
}
