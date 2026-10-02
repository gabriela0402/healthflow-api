using HealthFlow.Model.DTOs.Appointments;
using HealthFlow.Model.Entities;
using HealthFlow.Model.Enums;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Services;
using Moq;

namespace HealthFlow.Tests.Services;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository>
        _appointmentRepositoryMock;

    private readonly Mock<IPatientRepository>
        _patientRepositoryMock;

    private readonly Mock<IProfessionalRepository>
        _professionalRepositoryMock;

    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _appointmentRepositoryMock =
            new Mock<IAppointmentRepository>();

        _patientRepositoryMock =
            new Mock<IPatientRepository>();

        _professionalRepositoryMock =
            new Mock<IProfessionalRepository>();

        _service = new AppointmentService(
            _appointmentRepositoryMock.Object,
            _patientRepositoryMock.Object,
            _professionalRepositoryMock.Object);
    }

    [Fact]
    public async Task Deve_criar_agendamento_valido()
    {
        var patient = new Patient(
            "Mariana Oliveira",
            "mariana@email.com",
            "(11) 99999-8888",
            new DateTime(1995, 4, 18));

        var professional = new Professional(
            "Carlos Mendes",
            "CREFITO-12345",
            1);

        _patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        _professionalRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        _appointmentRepositoryMock
            .Setup(repository => repository.ExistsAtDateAsync(
                1,
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var request = new CreateAppointmentRequest(
            1,
            1,
            DateTime.Now.AddDays(1),
            "Primeira consulta");

        var result = await _service.CreateAsync(request);

        Assert.NotNull(result);
        Assert.Equal(1, result.PatientId);
        Assert.Equal(1, result.ProfessionalId);
        Assert.Equal("Scheduled", result.Status);
        Assert.Equal(
            "Primeira consulta",
            result.Notes);

        _appointmentRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Appointment>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _appointmentRepositoryMock.Verify(
            repository => repository.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Nao_deve_criar_agendamento_sem_paciente()
    {
        _patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        var request = new CreateAppointmentRequest(
            1,
            1,
            DateTime.Now.AddDays(1),
            null);

        var exception = await Assert.ThrowsAsync<
            BusinessRuleException>(
            () => _service.CreateAsync(request));

        Assert.Equal(
            "O paciente informado não existe.",
            exception.Message);

        _appointmentRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Appointment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Nao_deve_criar_agendamento_sem_profissional()
    {
        var patient = new Patient(
            "Mariana Oliveira",
            "mariana@email.com",
            "(11) 99999-8888",
            new DateTime(1995, 4, 18));

        _patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        _professionalRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Professional?)null);

        var request = new CreateAppointmentRequest(
            1,
            1,
            DateTime.Now.AddDays(1),
            null);

        var exception = await Assert.ThrowsAsync<
            BusinessRuleException>(
            () => _service.CreateAsync(request));

        Assert.Equal(
            "O profissional informado não existe.",
            exception.Message);

        _appointmentRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Appointment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Nao_deve_criar_agendamento_em_horario_ocupado()
    {
        var patient = new Patient(
            "Mariana Oliveira",
            "mariana@email.com",
            "(11) 99999-8888",
            new DateTime(1995, 4, 18));

        var professional = new Professional(
            "Carlos Mendes",
            "CREFITO-12345",
            1);

        _patientRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        _professionalRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(professional);

        _appointmentRepositoryMock
            .Setup(repository => repository.ExistsAtDateAsync(
                1,
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var request = new CreateAppointmentRequest(
            1,
            1,
            DateTime.Now.AddDays(1),
            null);

        var exception = await Assert.ThrowsAsync<
            BusinessRuleException>(
            () => _service.CreateAsync(request));

        Assert.Equal(
            "O profissional já possui um agendamento nesse horário.",
            exception.Message);

        _appointmentRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Appointment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Nao_deve_concluir_agendamento_cancelado()
    {
        var appointment = new Appointment(
            1,
            1,
            DateTime.Now.AddDays(1),
            "Consulta");

        appointment.Cancel();

        _appointmentRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        var exception = await Assert.ThrowsAsync<
            BusinessRuleException>(
            () => _service.CompleteAsync(1));

        Assert.Equal(
            "Um agendamento cancelado não pode ser concluído.",
            exception.Message);

        Assert.Equal(
            AppointmentStatus.Cancelled,
            appointment.Status);

        _appointmentRepositoryMock.Verify(
            repository => repository.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
