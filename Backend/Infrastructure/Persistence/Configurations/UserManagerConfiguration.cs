using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class UserManagerConfiguration : IEntityTypeConfiguration<UserManager>
{
    public void Configure(EntityTypeBuilder<UserManager> builder)
    {
        builder.Property(manager => manager.Created)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        /*** One to one ***/
        builder.HasOne(manager => manager.User)
            .WithOne(user => user.UserManager);
    }
}
