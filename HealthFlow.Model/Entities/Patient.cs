namespace HealthFlow.Model.Entities;

public class Patient
{
    public int Id { get; private set; }

    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string Phone { get; private set; } = string.Empty;

    public DateTime DateOfBirth { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public ICollection<Appointment> Appointments { get; private set; } =
        new List<Appointment>();

    protected Patient()
    {
    }

    public Patient(
        string fullName,
        string email,
        string phone,
        DateTime dateOfBirth)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        DateOfBirth = dateOfBirth;
        CreatedAt = DateTime.UtcNow;
    }
}
