using Domain.Specializations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Specializations;

internal class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
    public void Configure(EntityTypeBuilder<Specialization> builder)
    {
        builder.HasIndex(s => s.Name)
            .IsUnique();
        
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.Description)
            .HasMaxLength(250);
        
        builder.Metadata
            .FindSkipNavigation(nameof(Specialization.Doctors))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}