namespace HealthFlow.Model.Entities;

public class ClinicSettings
{
    public int Id { get; private set; }
    public string ClinicName { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public bool AppointmentReminders { get; private set; } = true;
    public bool AutomaticConfirmation { get; private set; }

    protected ClinicSettings() { }

    public ClinicSettings(string clinicName) => ClinicName = clinicName;

    public void Update(
        string clinicName, string? email, string? phone, string? address,
        bool appointmentReminders, bool automaticConfirmation)
    {
        ClinicName = clinicName;
        Email = email;
        Phone = phone;
        Address = address;
        AppointmentReminders = appointmentReminders;
        AutomaticConfirmation = automaticConfirmation;
    }
}