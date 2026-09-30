using HealthFlow.Model.Entities;

namespace HealthFlow.Model.DTOs.Professionals;

public record ProfessionalResponse(
	int Id,
	string FullName,
	string RegistrationNumber,
	int SpecialtyId,
	string SpecialtyName)
{
	public static ProfessionalResponse FromEntity(
		Professional professional)
	{
		return new ProfessionalResponse(
			professional.Id,
			professional.FullName,
			professional.RegistrationNumber,
			professional.SpecialtyId,
			professional.Specialty.Name);
	}
}
