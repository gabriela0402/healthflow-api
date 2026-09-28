namespace HealthFlow.Model.Entities;

public class Professional
{
    public Guid Id { get; private set; }

    public string FullName { get; private set; } = string.Empty;

    public string RegistrationNumber { get; private set; } = string.Empty;

    public Guid SpecialtyId { get; private set; }

    public Specialty Specialty { get; private set; } = null!;

    public ICollection<Appointment> Appointments { get; private set; } =
        new List<Appointment>();

    protected Professional()
    {
    }

    public Professional(
        string fullName,
        string registrationNumber,
        Guid specialtyId)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        RegistrationNumber = registrationNumber;
        SpecialtyId = specialtyId;
    }
}
