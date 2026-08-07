using Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Managers;

internal class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.Property(m => m.Position)
            .HasMaxLength(100);
        
        /*** One-to-one ***/
        builder.HasOne(m => m.User)
            .WithOne()
            .HasForeignKey<Manager>(m => m.UserId);
    }
}
