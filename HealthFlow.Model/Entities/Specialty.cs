namespace HealthFlow.Model.Entities;

public class Specialty
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public ICollection<Professional> Professionals { get; private set; } =
        new List<Professional>();

    protected Specialty()
    {
    }

    public Specialty(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
