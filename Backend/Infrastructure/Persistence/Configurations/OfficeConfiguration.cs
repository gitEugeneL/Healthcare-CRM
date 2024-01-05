using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OfficeConfiguration :  IEntityTypeConfiguration<Office>
{
    public void Configure(EntityTypeBuilder<Office> builder)
    {
        builder.HasIndex(o => o.Number)
            .IsUnique();

        builder.Property(o => o.Number)
            .IsRequired();

        builder.Property(o => o.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
