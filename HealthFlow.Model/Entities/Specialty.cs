namespace HealthFlow.Model.Entities;

public class Specialty
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public ICollection<Professional> Professionals { get; private set; } =
        new List<Professional>();

    protected Specialty()
    {
    }

    public Specialty(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
