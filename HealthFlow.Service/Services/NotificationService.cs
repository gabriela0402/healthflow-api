using HealthFlow.Model.DTOs.Notifications;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;

namespace HealthFlow.Service.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notifications;
    private readonly IPatientRepository _patients;
    private readonly IClinicSettingsRepository _settings;

    public NotificationService(
        INotificationRepository notifications,
        IPatientRepository patients,
        IClinicSettingsRepository settings)
    {
        _notifications = notifications;
        _patients = patients;
        _settings = settings;
    }

    public async Task NotifyAppointmentAsync(
        Patient patient, string title, string description,
        CancellationToken cancellationToken = default)
    {
        if (!patient.NotifyAppointmentUpdates) return;

        await _notifications.AddAsync(
            new Notification(patient.Id, "appointment", title, description),
            cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationResponse>> GetMineAsync(
        string email, CancellationToken cancellationToken = default)
    {
        var patient = await GetPatientAsync(email, cancellationToken);
        var list = await _notifications.GetByPatientIdAsync(patient.Id, cancellationToken);

        return list.Select(NotificationResponse.FromEntity).ToList();
    }

    public async Task MarkAsReadAsync(
        string email, int id, CancellationToken cancellationToken = default)
    {
        var patient = await GetPatientAsync(email, cancellationToken);
        var notification = await _notifications.GetByIdAsync(id, cancellationToken);

        if (notification is null || notification.PatientId != patient.Id)
        {
            throw new BusinessRuleException("Notificação não encontrada.");
        }

        notification.MarkAsRead();
        await _notifications.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkAllAsReadAsync(
        string email, CancellationToken cancellationToken = default)
    {
        var patient = await GetPatientAsync(email, cancellationToken);
        var unread = await _notifications.GetUnreadByPatientIdAsync(patient.Id, cancellationToken);

        foreach (var notification in unread) notification.MarkAsRead();

        await _notifications.SaveChangesAsync(cancellationToken);
    }

    public async Task<NotificationPreferencesResponse> GetPreferencesAsync(
        string email, CancellationToken cancellationToken = default)
    {
        var patient = await GetPatientAsync(email, cancellationToken);

        return new NotificationPreferencesResponse(
            patient.NotifyEmail, patient.NotifyReminders, patient.NotifyAppointmentUpdates);
    }

    public async Task<NotificationPreferencesResponse> UpdatePreferencesAsync(
        string email, UpdateNotificationPreferencesRequest request,
        CancellationToken cancellationToken = default)
    {
        var patient = await GetPatientAsync(email, cancellationToken);

        patient.UpdateNotificationPreferences(
            request.Email, request.Reminders, request.AppointmentUpdates);

        await _patients.SaveChangesAsync(cancellationToken);

        return new NotificationPreferencesResponse(
            patient.NotifyEmail, patient.NotifyReminders, patient.NotifyAppointmentUpdates);
    }

    public async Task SendRemindersAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _settings.GetAsync(cancellationToken);

        if (settings?.AppointmentReminders == false) return;

        var now = DateTime.Now;
        var appointments = await _notifications.GetAppointmentsForReminderAsync(
            now, now.AddHours(24), cancellationToken);

        var created = false;

        foreach (var appointment in appointments.Where(a => a.Patient.NotifyReminders))
        {
            var when = $"{appointment.ScheduledAt:dd/MM/yyyy} às {appointment.ScheduledAt:HH:mm}";

            await _notifications.AddAsync(
                new Notification(
                    appointment.PatientId,
                    "reminder",
                    "Lembrete de consulta",
                    $"Você tem uma consulta com {appointment.Professional.FullName} em {when}.",
                    appointment.Id),
                cancellationToken);

            created = true;
        }

        if (created) await _notifications.SaveChangesAsync(cancellationToken);
    }

    private async Task<Patient> GetPatientAsync(string email, CancellationToken cancellationToken)
    {
        return await _patients.GetByEmailAsync(email.Trim().ToLowerInvariant(), cancellationToken)
               ?? throw new BusinessRuleException(
                   "Paciente não encontrado para o usuário logado.");
    }
}