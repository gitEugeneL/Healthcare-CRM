using Domain.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Addresses;

internal sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(a => a.Province)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Hose)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(a => a.Apartment)
            .HasMaxLength(10);

        builder.Property(a => a.PostalCode)
            .IsRequired()
            .HasMaxLength(10);
    }
}
