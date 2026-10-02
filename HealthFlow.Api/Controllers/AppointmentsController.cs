using HealthFlow.Model.DTOs.Appointments;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

}
