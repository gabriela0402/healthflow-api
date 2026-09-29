using HealthFlow.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthFlow.Data.Configurations;

public class ProfessionalConfiguration
    : IEntityTypeConfiguration<Professional>
{
    public void Configure(EntityTypeBuilder<Professional> builder)
    {
        builder.ToTable("Professionals");

        builder.HasKey(professional => professional.Id);

        builder.Property(professional => professional.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(professional => professional.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasOne(professional => professional.Specialty)
            .WithMany(specialty => specialty.Professionals)
            .HasForeignKey(professional => professional.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(professional =>
            new
            {
                professional.RegistrationNumber,
                professional.SpecialtyId
            })
            .IsUnique();
    }
}
