using HealthFlow.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthFlow.Data.Configurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        builder.ToTable("Specialties");

        builder.HasKey(specialty => specialty.Id);

        builder.Property(specialty => specialty.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(specialty => specialty.Name)
            .IsUnique();
    }
}
