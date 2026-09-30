namespace HealthFlow.Model.Entities;

public class Professional
{
    public int Id { get; private set; }

    public string FullName { get; private set; } = string.Empty;

    public string RegistrationNumber { get; private set; } = string.Empty;

    public int SpecialtyId { get; private set; }

    public Specialty Specialty { get; private set; } = null!;

    public ICollection<Appointment> Appointments { get; private set; } =
        new List<Appointment>();

    protected Professional()
    {
    }

    public Professional(
        string fullName,
        string registrationNumber,
        int specialtyId)
    {
        FullName = fullName;
        RegistrationNumber = registrationNumber;
        SpecialtyId = specialtyId;
    }
}
