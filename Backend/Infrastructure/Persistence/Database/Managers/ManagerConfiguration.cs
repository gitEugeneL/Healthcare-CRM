using Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Database.Managers;

internal class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasIndex(m => m.UserId)
            .IsUnique();
        
        builder.Property(m => m.Position)
            .HasMaxLength(100);
        
        /*** One-to-one ***/
        builder.HasOne(m => m.User)
            .WithOne()
            .IsRequired()
            .HasForeignKey<Manager>(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
