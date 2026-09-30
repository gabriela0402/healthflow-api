using HealthFlow.Model.DTOs.Appointments;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;

namespace HealthFlow.Service.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IProfessionalRepository _professionalRepository;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IProfessionalRepository professionalRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _professionalRepository = professionalRepository;
    }

    public async Task<AppointmentResponse> CreateAsync(
        CreateAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.PatientId <= 0)
        {
            throw new BusinessRuleException(
                "O paciente informado é inválido.");
        }

        if (request.ProfessionalId <= 0)
        {
            throw new BusinessRuleException(
                "O profissional informado é inválido.");
        }

        if (request.ScheduledAt <= DateTime.Now)
        {
            throw new BusinessRuleException(
                "A data do agendamento deve estar no futuro.");
        }

        var patient = await _patientRepository.GetByIdAsync(
            request.PatientId,
            cancellationToken);

        if (patient is null)
        {
            throw new BusinessRuleException(
                "O paciente informado não existe.");
        }

        var professional =
            await _professionalRepository.GetByIdAsync(
                request.ProfessionalId,
                cancellationToken);

        if (professional is null)
        {
            throw new BusinessRuleException(
                "O profissional informado não existe.");
        }

        var scheduledAt = request.ScheduledAt
            .ToLocalTime()
            .AddSeconds(-request.ScheduledAt.Second);

        var alreadyExists =
            await _appointmentRepository.ExistsAtDateAsync(
                request.ProfessionalId,
                scheduledAt,
                cancellationToken);

        if (alreadyExists)
        {
            throw new BusinessRuleException(
                "O profissional já possui um agendamento nesse horário.");
        }

        var appointment = new Appointment(
            request.PatientId,
            request.ProfessionalId,
            scheduledAt,
            request.Notes);

        await _appointmentRepository.AddAsync(
            appointment,
            cancellationToken);

        await _appointmentRepository.SaveChangesAsync(
            cancellationToken);

        return AppointmentResponse.FromEntity(appointment);
    }

    public async Task<AppointmentResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new BusinessRuleException(
                "O identificador do agendamento é inválido.");
        }

        var appointment =
            await _appointmentRepository.GetByIdAsync(
                id,
                cancellationToken);

        return appointment is null
            ? null
            : AppointmentResponse.FromEntity(appointment);
    }

    public async Task<IReadOnlyList<AppointmentResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var appointments =
            await _appointmentRepository.GetAllAsync(
                cancellationToken);

        return appointments
            .Select(AppointmentResponse.FromEntity)
            .ToList();
    }
}
