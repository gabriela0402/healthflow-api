namespace HealthFlow.Model.Entities;

public class Patient
{
    public int Id { get; private set; }

    public string FullName { get; private set; } =
        string.Empty;

    public string Email { get; private set; } =
        string.Empty;

    public string? Phone { get; private set; }

    public DateTime? DateOfBirth { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public ICollection<Appointment> Appointments
    {
        get;
        private set;
    } = new List<Appointment>();

    protected Patient()
    {
    }

    public Patient(
        string fullName,
        string email,
        string? phone = null,
        DateTime? dateOfBirth = null)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        DateOfBirth = dateOfBirth;
        CreatedAt = DateTime.UtcNow;
    }

    public string? Gender { get; private set; }
    public string? Cpf { get; private set; }
    public string? Address { get; private set; }
    public string? EmergencyContactName { get; private set; }
    public string? EmergencyContactPhone { get; private set; }

    public void UpdateProfile(
        string fullName,
        string? phone,
        DateTime? dateOfBirth,
        string? gender,
        string? cpf,
        string? address,
        string? emergencyContactName,
        string? emergencyContactPhone)
    {
        FullName = fullName;
        Phone = phone;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Cpf = cpf;
        Address = address;
        EmergencyContactName = emergencyContactName;
        EmergencyContactPhone = emergencyContactPhone;
    }
}
