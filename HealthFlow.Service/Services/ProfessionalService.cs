using HealthFlow.Model.DTOs.Professionals;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;

namespace HealthFlow.Service.Services;

public class ProfessionalService : IProfessionalService
{
    private readonly IProfessionalRepository
        _professionalRepository;

    public ProfessionalService(
        IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    public async Task<ProfessionalResponse> CreateAsync(
        CreateProfessionalRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new BusinessRuleException(
                "O nome do profissional é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(
            request.RegistrationNumber))
        {
            throw new BusinessRuleException(
                "O registro profissional é obrigatório.");
        }

        if (request.SpecialtyId <= 0)
        {
            throw new BusinessRuleException(
                "A especialidade é obrigatória.");
        }

        var registrationNumber =
            request.RegistrationNumber.Trim();

        var existingProfessional =
            await _professionalRepository
                .GetByRegistrationNumberAsync(
                    registrationNumber,
                    cancellationToken);

        if (existingProfessional is not null)
        {
            throw new BusinessRuleException(
                "Esse registro profissional já está cadastrado.");
        }

        var professional = new Professional(
            request.FullName.Trim(),
            registrationNumber,
            request.SpecialtyId);

        await _professionalRepository.AddAsync(
            professional,
            cancellationToken);

        await _professionalRepository.SaveChangesAsync(
            cancellationToken);

        var createdProfessional =
            await _professionalRepository.GetByIdAsync(
                professional.Id,
                cancellationToken);

        return ProfessionalResponse.FromEntity(
            createdProfessional!);
    }

    public async Task<ProfessionalResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new BusinessRuleException(
                "O identificador do profissional é inválido.");
        }

        var professional =
            await _professionalRepository.GetByIdAsync(
                id,
                cancellationToken);

        return professional is null
            ? null
            : ProfessionalResponse.FromEntity(professional);
    }

    public async Task<IReadOnlyList<ProfessionalResponse>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        var professionals =
            await _professionalRepository.GetAllAsync(
                cancellationToken);

        return professionals
            .Select(ProfessionalResponse.FromEntity)
            .ToList();
    }
}
