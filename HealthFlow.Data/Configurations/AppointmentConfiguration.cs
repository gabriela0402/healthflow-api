using HealthFlow.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthFlow.Data.Configurations;

public class AppointmentConfiguration
    : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(appointment => appointment.Id);

        builder.Property(appointment => appointment.ScheduledAt)
            .IsRequired();

        builder.Property(appointment => appointment.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(appointment => appointment.Notes)
            .HasMaxLength(500);

        builder.Property(appointment => appointment.CreatedAt)
            .IsRequired();

        builder.HasOne(appointment => appointment.Patient)
            .WithMany(patient => patient.Appointments)
            .HasForeignKey(appointment => appointment.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(appointment => appointment.Professional)
            .WithMany(professional => professional.Appointments)
            .HasForeignKey(appointment => appointment.ProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(appointment =>
            new
            {
                appointment.ProfessionalId,
                appointment.ScheduledAt
            })
            .IsUnique();
    }
}
