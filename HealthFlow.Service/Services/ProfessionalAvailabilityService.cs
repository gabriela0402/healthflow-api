using HealthFlow.Model.DTOs.ProfessionalAvailabilities;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;

namespace HealthFlow.Service.Services;

public class ProfessionalAvailabilityService
    : IProfessionalAvailabilityService
{
    private readonly IProfessionalAvailabilityRepository
        _availabilityRepository;

    private readonly IProfessionalRepository
        _professionalRepository;

    public ProfessionalAvailabilityService(
        IProfessionalAvailabilityRepository
            availabilityRepository,
        IProfessionalRepository professionalRepository)
    {
        _availabilityRepository =
            availabilityRepository;

        _professionalRepository =
            professionalRepository;
    }

    public async Task<ProfessionalAvailabilityResponse>
        CreateAsync(
            int professionalId,
            CreateProfessionalAvailabilityRequest request,
            CancellationToken cancellationToken = default)
    {
        if (professionalId <= 0)
        {
            throw new BusinessRuleException(
                "O identificador do profissional é inválido.");
        }

        if (request.StartTime >= request.EndTime)
        {
            throw new BusinessRuleException(
                "O horário inicial deve ser anterior ao horário final.");
        }

        if (request.SlotDurationInMinutes <= 0)
        {
            throw new BusinessRuleException(
                "A duração da consulta deve ser maior que zero.");
        }

        var totalMinutes =
            (request.EndTime - request.StartTime)
            .TotalMinutes;

        if (request.SlotDurationInMinutes > totalMinutes)
        {
            throw new BusinessRuleException(
                "A duração da consulta não pode ser maior que o intervalo disponível.");
        }

        if (totalMinutes %
            request.SlotDurationInMinutes != 0)
        {
            throw new BusinessRuleException(
                "O intervalo deve ser divisível pela duração da consulta.");
        }

        var professional =
            await _professionalRepository.GetByIdAsync(
                professionalId,
                cancellationToken);

        if (professional is null)
        {
            throw new BusinessRuleException(
                "O profissional informado não existe.");
        }

        var alreadyExists =
            await _availabilityRepository
                .ExistsForProfessionalAndDayAsync(
                    professionalId,
                    request.DayOfWeek,
                    cancellationToken);

        if (alreadyExists)
        {
            throw new BusinessRuleException(
                "Já existe uma disponibilidade cadastrada para esse profissional neste dia.");
        }

        var availability =
            new ProfessionalAvailability(
                professionalId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                request.SlotDurationInMinutes);

        await _availabilityRepository.AddAsync(
            availability,
            cancellationToken);

        await _availabilityRepository.SaveChangesAsync(
            cancellationToken);

        return ProfessionalAvailabilityResponse
            .FromEntity(availability);
    }

    public async Task<
        IReadOnlyList<ProfessionalAvailabilityResponse>>
        GetByProfessionalIdAsync(
            int professionalId,
            CancellationToken cancellationToken = default)
    {
        if (professionalId <= 0)
        {
            throw new BusinessRuleException(
                "O identificador do profissional é inválido.");
        }

        var professional =
            await _professionalRepository.GetByIdAsync(
                professionalId,
                cancellationToken);

        if (professional is null)
        {
            throw new BusinessRuleException(
                "O profissional informado não existe.");
        }

        var availabilities =
            await _availabilityRepository
                .GetByProfessionalIdAsync(
                    professionalId,
                    cancellationToken);

        return availabilities
            .Select(
                ProfessionalAvailabilityResponse
                    .FromEntity)
            .ToList();
    }
}
