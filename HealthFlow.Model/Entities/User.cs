namespace HealthFlow.Model.Entities;

public class User
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Role { get; private set; } = "Patient";

    public DateTime CreatedAt { get; private set; }

    protected User()
    {
    }

    public User(
        string name,
        string email,
        string passwordHash,
        string role = "Patient")
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void SetRole(string role)
    {
        Role = role;
    }
    public void SetName(string name)
    {
        Name = name;
    }
}

