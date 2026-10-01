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
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .HasIndex(user => user.Email)
        .IsUnique();

        modelBuilder.Entity<Appointment>()
        .Property(appointment => appointment.Status)
        .HasConversion<string>();

        base.OnModelCreating(modelBuilder);
    }
}
