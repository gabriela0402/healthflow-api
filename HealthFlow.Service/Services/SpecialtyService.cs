using HealthFlow.Model.DTOs.Specialties;
using HealthFlow.Model.Entities;
using HealthFlow.Repository.Interfaces;
using HealthFlow.Service.Exceptions;
using HealthFlow.Service.Interfaces;

namespace HealthFlow.Service.Services;

public sealed class SpecialtyService : ISpecialtyService
{
    private readonly ISpecialtyRepository _specialtyRepository;

    public SpecialtyService(ISpecialtyRepository specialtyRepository)
    {
        _specialtyRepository = specialtyRepository;
    }

    public async Task<IReadOnlyList<SpecialtyResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var specialties = await _specialtyRepository.GetAllAsync(cancellationToken);

        return specialties
            .Select(SpecialtyResponse.FromEntity)
            .ToList();
    }

    public async Task<SpecialtyResponse> CreateAsync(
        CreateSpecialtyRequest request, CancellationToken cancellationToken = default)
    {
        var name = ValidateName(request.Name);

        if (await _specialtyRepository.ExistsByNameAsync(name, null, cancellationToken))
        {
            throw new BusinessRuleException("Já existe uma especialidade com este nome.");
        }

        var specialty = new Specialty(name, NullIfEmpty(request.Description));

        await _specialtyRepository.AddAsync(specialty, cancellationToken);
        await _specialtyRepository.SaveChangesAsync(cancellationToken);

        return SpecialtyResponse.FromEntity(specialty);
    }

    public async Task<SpecialtyResponse> UpdateAsync(
        int id, UpdateSpecialtyRequest request, CancellationToken cancellationToken = default)
    {
        var specialty = await _specialtyRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessRuleException("Especialidade não encontrada.");

        var name = ValidateName(request.Name);

        if (await _specialtyRepository.ExistsByNameAsync(name, id, cancellationToken))
        {
            throw new BusinessRuleException("Já existe uma especialidade com este nome.");
        }

        specialty.Update(name, NullIfEmpty(request.Description));
        await _specialtyRepository.SaveChangesAsync(cancellationToken);

        return SpecialtyResponse.FromEntity(specialty);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var specialty = await _specialtyRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new BusinessRuleException("Especialidade não encontrada.");

        if (await _specialtyRepository.HasProfessionalsAsync(id, cancellationToken))
        {
            throw new BusinessRuleException(
                "Não é possível excluir: existem profissionais vinculados a esta especialidade.");
        }

        _specialtyRepository.Remove(specialty);
        await _specialtyRepository.SaveChangesAsync(cancellationToken);
    }

    private static string ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 3)
        {
            throw new BusinessRuleException(
                "O nome da especialidade deve possuir pelo menos 3 caracteres.");
        }

        return name.Trim();
    }

    private static string? NullIfEmpty(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}