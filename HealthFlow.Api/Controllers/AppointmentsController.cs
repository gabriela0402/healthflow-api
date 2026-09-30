using HealthFlow.Model.DTOs.Appointments;
using HealthFlow.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
}
