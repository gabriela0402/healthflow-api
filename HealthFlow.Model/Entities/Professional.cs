namespace HealthFlow.Model.Entities;

public class Professional
{
    public int Id { get; private set; }

    public string FullName { get; private set; } = string.Empty;
    public string? Email { get; private set; }

    public string RegistrationNumber { get; private set; } =
        string.Empty;

    public int SpecialtyId { get; private set; }

    public Specialty Specialty { get; private set; } = null!;
    public string? Phone { get; private set; }

    public ICollection<Appointment> Appointments
    {
        get;
        private set;
    } = new List<Appointment>();

    public ICollection<ProfessionalAvailability> Availabilities
    {
        get;
        private set;
    } = new List<ProfessionalAvailability>();

    protected Professional()
    {
    }

    public Professional(
    string fullName,
    string registrationNumber,
    int specialtyId,
    string? email = null)
    {
        FullName = fullName;
        RegistrationNumber = registrationNumber;
        SpecialtyId = specialtyId;
        Email = email;
    }

    public void Update(string fullName, string registrationNumber, int specialtyId)
    {
        FullName = fullName;
        RegistrationNumber = registrationNumber;
        SpecialtyId = specialtyId;
    }

    public void SetEmail(string email)
    {
        Email = email;
    }

    public void UpdateProfile(string fullName, string? phone)
    {
        FullName = fullName;
        Phone = phone;
    }
}
