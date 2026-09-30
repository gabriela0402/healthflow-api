using HealthFlow.Model.DTOs.Patients;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
}
