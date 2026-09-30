using HealthFlow.Model.DTOs.Specialties;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;

namespace HealthFlow.Service.Services;

public sealed class SpecialtyService : ISpecialtyService
{
    private readonly ISpecialtyRepository _specialtyRepository;

    public SpecialtyService(
        ISpecialtyRepository specialtyRepository)
    {
        _specialtyRepository = specialtyRepository;
    }

    public async Task<SpecialtyResponse> CreateAsync(
        CreateSpecialtyRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new BusinessRuleException(
                "O nome da especialidade é obrigatório.");
        }

        var name = request.Name.Trim();

        var existingSpecialty =
            await _specialtyRepository.GetByNameAsync(
                name,
                cancellationToken);

        if (existingSpecialty is not null)
        {
            throw new BusinessRuleException(
                "Essa especialidade já está cadastrada.");
        }

        var specialty = new Specialty(name);

        await _specialtyRepository.AddAsync(
            specialty,
            cancellationToken);

        await _specialtyRepository.SaveChangesAsync(
            cancellationToken);

        return SpecialtyResponse.FromEntity(specialty);
    }

    public async Task<IReadOnlyList<SpecialtyResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var specialties = await _specialtyRepository.GetAllAsync(
            cancellationToken);

        return specialties
            .Select(SpecialtyResponse.FromEntity)
            .ToList();
    }
}
