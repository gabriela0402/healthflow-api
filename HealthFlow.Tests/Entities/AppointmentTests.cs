using HealthFlow.Model.Entities;
using HealthFlow.Model.Enums;

namespace HealthFlow.Tests.Entities;

public class AppointmentTests
{
    [Fact]
    public void Deve_criar_agendamento_com_status_scheduled()
    {
        var appointment = new Appointment(
            patientId: 1,
            professionalId: 1,
            scheduledAt: DateTime.Now.AddDays(1),
            notes: "Primeira consulta");

        Assert.Equal(1, appointment.PatientId);
        Assert.Equal(1, appointment.ProfessionalId);
        Assert.Equal(
            AppointmentStatus.Scheduled,
            appointment.Status);
        Assert.Equal(
            "Primeira consulta",
            appointment.Notes);
    }

    [Fact]
    public void Deve_confirmar_agendamento()
    {
        var appointment = CreateAppointment();

        appointment.Confirm();

        Assert.Equal(
            AppointmentStatus.Confirmed,
            appointment.Status);
    }

    [Fact]
    public void Deve_cancelar_agendamento()
    {
        var appointment = CreateAppointment();

        appointment.Cancel();

        Assert.Equal(
            AppointmentStatus.Cancelled,
            appointment.Status);
    }

    [Fact]
    public void Deve_concluir_agendamento()
    {
        var appointment = CreateAppointment();

        appointment.Complete();

        Assert.Equal(
            AppointmentStatus.Completed,
            appointment.Status);
    }

    private static Appointment CreateAppointment()
    {
        return new Appointment(
            patientId: 1,
            professionalId: 1,
            scheduledAt: DateTime.Now.AddDays(1),
            notes: null);
    }
}
