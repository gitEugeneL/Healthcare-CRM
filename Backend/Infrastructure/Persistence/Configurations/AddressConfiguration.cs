using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(address => address.Province)
            .HasMaxLength(100);

        builder.Property(address => address.City)
            .HasMaxLength(100);

        builder.Property(address => address.Street)
            .HasMaxLength(100);

        builder.Property(address => address.Hose)
            .HasMaxLength(10);

        builder.Property(address => address.Apartment)
            .HasMaxLength(10);

        builder.Property(address => address.PostalCode)
            .HasMaxLength(10);
        
        builder.Property(doctor => doctor.Created)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); 
        
        /*** One to one ***/
        builder.HasOne(address => address.UserPatient)
            .WithOne(patient => patient.Address);
    }
}
