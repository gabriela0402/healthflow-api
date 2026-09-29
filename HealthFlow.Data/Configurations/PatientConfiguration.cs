using HealthFlow.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthFlow.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(patient => patient.Id);

        builder.Property(patient => patient.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(patient => patient.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(patient => patient.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(patient => patient.DateOfBirth)
            .IsRequired();

        builder.Property(patient => patient.CreatedAt)
            .IsRequired();

        builder.HasIndex(patient => patient.Email)
            .IsUnique();
    }
}
