using HealthFlow.Model.DTOs.Appointments;
using HealthFlow.Model.DTOs.Patients;
using HealthFlow.Model.Entities;
using HealthFlow.Model.Enums;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;
namespace HealthFlow.Service.Services;


public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IClinicSettingsRepository _settingsRepository; 

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IProfessionalRepository professionalRepository,
        IClinicSettingsRepository settingsRepository)             
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _professionalRepository = professionalRepository;
        _settingsRepository = settingsRepository;           
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

        var settings = await _settingsRepository.GetAsync(cancellationToken);

        if (settings?.AutomaticConfirmation == true)
        {
            appointment.Confirm();
        }

        await _appointmentRepository.AddAsync(
            appointment,
            cancellationToken);

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

    public async Task<AppointmentResponse> ConfirmAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        var appointment = await GetAppointmentOrThrowAsync(
            id,
            cancellationToken);

        appointment.Confirm();

        await _appointmentRepository.SaveChangesAsync(
            cancellationToken);

        return AppointmentResponse.FromEntity(appointment);
    }

    public async Task<AppointmentResponse> CancelAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var appointment = await GetAppointmentOrThrowAsync(
            id,
            cancellationToken);

        appointment.Cancel();

        await _appointmentRepository.SaveChangesAsync(
            cancellationToken);

        return AppointmentResponse.FromEntity(appointment);
    }

    public async Task<AppointmentResponse> CompleteAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        var appointment = await GetAppointmentOrThrowAsync(
            id,
            cancellationToken);

        if (appointment.Status == AppointmentStatus.Cancelled)
        {
            throw new BusinessRuleException(
                "Um agendamento cancelado não pode ser concluído.");
        }

        appointment.Complete();

        await _appointmentRepository.SaveChangesAsync(
            cancellationToken);

        return AppointmentResponse.FromEntity(appointment);
    }


    private async Task<Appointment> GetAppointmentOrThrowAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            throw new BusinessRuleException(
                "O identificador do agendamento é inválido.");
        }

        var appointment = await _appointmentRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (appointment is null)
        {
            throw new BusinessRuleException(
                "Agendamento não encontrado.");
        }

        return appointment;
    }
    public async Task<IReadOnlyList<AppointmentListItemResponse>> GetMyAppointmentsAsync(
    string email,
    CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByEmailAsync(
            email.Trim().ToLowerInvariant(),
            cancellationToken);

        if (patient is null)
        {
            throw new BusinessRuleException(
                "Paciente não encontrado para o usuário logado.");
        }

        var appointments = await _appointmentRepository.GetByPatientIdAsync(
            patient.Id,
            cancellationToken);

        return appointments
            .Select(AppointmentListItemResponse.FromEntity)
            .ToList();
    }

    public async Task<int> GetPatientIdByEmailAsync(
    string email,
    CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByEmailAsync(
            email.Trim().ToLowerInvariant(),
            cancellationToken);

        if (patient is null)
        {
            throw new BusinessRuleException(
                "Paciente não encontrado para o usuário logado.");
        }

        return patient.Id;
    }

    public async Task<IReadOnlyList<AppointmentAdminItemResponse>> GetOverviewAsync(
    CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllDetailedAsync(
            cancellationToken);

        return appointments
            .Select(AppointmentAdminItemResponse.FromEntity)
            .ToList();
    }

    public async Task<IReadOnlyList<ProfessionalPatientResponse>> GetMyPatientsAsync(
    string email,
    CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByEmailAsync(
            email.Trim().ToLowerInvariant(),
            cancellationToken);

        if (professional is null)
        {
            throw new BusinessRuleException(
                "Profissional não encontrado para o usuário logado.");
        }

        var appointments = await _appointmentRepository.GetByProfessionalIdAsync(
            professional.Id,
            cancellationToken);

        var now = DateTime.Now;

        return appointments
            .GroupBy(a => a.PatientId)
            .Select(group => ProfessionalPatientResponse.FromAppointments(group.ToList(), now))
            .OrderBy(p => p.FullName)
            .ToList();
    }

    public async Task<IReadOnlyList<AppointmentProfessionalItemResponse>> GetMyScheduleAsync(
    string email,
    CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByEmailAsync(
            email.Trim().ToLowerInvariant(),
            cancellationToken)
            ?? throw new BusinessRuleException(
                "Profissional não encontrado para o usuário logado.");

        var appointments = await _appointmentRepository.GetByProfessionalIdAsync(
            professional.Id,
            cancellationToken);

        return appointments
            .Select(AppointmentProfessionalItemResponse.FromEntity)
            .ToList();
    }

    public async Task EnsureProfessionalOwnsAsync(
        int appointmentId,
        string email,
        CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByEmailAsync(
            email.Trim().ToLowerInvariant(),
            cancellationToken);

        var appointment = await _appointmentRepository.GetByIdAsync(
            appointmentId,
            cancellationToken);

        if (professional is null || appointment is null ||
            appointment.ProfessionalId != professional.Id)
        {
            throw new BusinessRuleException(
                "Você não tem permissão para alterar este agendamento.");
        }
    }

}
