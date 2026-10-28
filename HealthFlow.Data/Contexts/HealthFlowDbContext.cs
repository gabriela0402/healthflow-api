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

    public DbSet<Patient> Patients =>
        Set<Patient>();

    public DbSet<Professional> Professionals =>
        Set<Professional>();

    public DbSet<Specialty> Specialties =>
        Set<Specialty>();

    public DbSet<Appointment> Appointments =>
        Set<Appointment>();

    public DbSet<ProfessionalAvailability>
        ProfessionalAvailabilities =>
        Set<ProfessionalAvailability>();

    public DbSet<User> Users =>
        Set<User>();

    public DbSet<ClinicSettings> Settings => 
        Set<ClinicSettings>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<Appointment>()
            .Property(appointment => appointment.Status)
            .HasConversion<string>();

        modelBuilder.Entity<ProfessionalAvailability>()
            .HasOne(availability => availability.Professional)
            .WithMany(professional =>
                professional.Availabilities)
            .HasForeignKey(availability =>
                availability.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProfessionalAvailability>()
            .Property(availability =>
                availability.DayOfWeek)
            .HasConversion<string>();

        modelBuilder.Entity<Professional>()
            .HasIndex(professional => professional.Email)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
