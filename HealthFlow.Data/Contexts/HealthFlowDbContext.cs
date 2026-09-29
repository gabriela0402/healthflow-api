using HealthFlow.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthFlow.Data.Contexts;

public class HealthFlowDbContext : DbContext
{
    public HealthFlowDbContext(
        DbContextOptions<HealthFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<Professional> Professionals => Set<Professional>();

    public DbSet<Specialty> Specialties => Set<Specialty>();

    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(HealthFlowDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
