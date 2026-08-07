using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(user => user.Email)
            .IsUnique();
        
        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(user => user.FirstName)
            .HasMaxLength(250);

        builder.Property(user => user.LastName)
            .HasMaxLength(250);
        
        builder.Property(user => user.Phone)
            .HasMaxLength(50);
        
        builder.Property(user => user.PasswordHash)
            .IsRequired();
        
        builder.Property(user => user.PasswordSalt)
            .IsRequired();
        
        builder.Property(user => user.Role)
            .IsRequired()
            .HasConversion<string>();
        
        /*** Many-to-one relation ***/
        builder.HasMany(user => user.RefreshTokens)
            .WithOne(refreshToken => refreshToken.User)
            .HasForeignKey(refreshToken => refreshToken.UserId); 
    }
}
