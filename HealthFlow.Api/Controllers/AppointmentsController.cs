using HealthFlow.Model.DTOs.Appointments;
using HealthFlow.Model.DTOs.Patients;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(
        IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AppointmentResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAllAsync(
            cancellationToken);

        return Ok(appointments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.GetByIdAsync(
            id,
            cancellationToken);

        if (appointment is null)
        {
            return NotFound("Agendamento não encontrado.");
        }

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentResponse>> Create(
        CreateAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        if (User.IsInRole("Admin") is false)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                return Unauthorized();
            }

            var patientId = await _appointmentService.GetPatientIdByEmailAsync(
                email,
                cancellationToken);

            request = request with { PatientId = patientId };
        }

        var appointment = await _appointmentService.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = appointment.Id },
            appointment);
    }

    [HttpPut("{id:int}/confirm")]
    public async Task<ActionResult<AppointmentResponse>> Confirm(
    int id,
    CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.ConfirmAsync(
            id,
            cancellationToken);

        return Ok(appointment);
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<ActionResult<AppointmentResponse>> Cancel(
        int id,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.CancelAsync(
            id,
            cancellationToken);

        return Ok(appointment);
    }

    [HttpPut("{id:int}/complete")]
    public async Task<ActionResult<AppointmentResponse>> Complete(
        int id,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.CompleteAsync(
            id,
            cancellationToken);

        return Ok(appointment);
    }

    [HttpGet("me")]
    public async Task<ActionResult<IReadOnlyList<AppointmentListItemResponse>>> GetMine(
    CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(ClaimTypes.Email)
            ?? User.FindFirstValue("email");

        if (string.IsNullOrWhiteSpace(email))
        {
            return Unauthorized();
        }

        var appointments = await _appointmentService.GetMyAppointmentsAsync(
            email,
            cancellationToken);

        return Ok(appointments);
    }

    [HttpGet("overview")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IReadOnlyList<AppointmentAdminItemResponse>>> GetOverview(
    CancellationToken cancellationToken)
    {
        return Ok(await _appointmentService.GetOverviewAsync(cancellationToken));
    }

    [HttpGet("my-patients")]
    [Authorize(Roles = "Professional")]
    public async Task<ActionResult<IReadOnlyList<ProfessionalPatientResponse>>> GetMyPatients(
    CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(email))
        {
            return Unauthorized();
        }

        return Ok(await _appointmentService.GetMyPatientsAsync(email, cancellationToken));
    }

}
