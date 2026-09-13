using Domain.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.RefreshTokens;

internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(rt => rt.Id).ValueGeneratedNever();
        
        builder.HasIndex(r => r.Token);

        builder.HasIndex(r => r.UserId);
        
        builder.Property(r => r.Token)
            .IsRequired();
        
        builder.Property(r => r.Expires)
            .IsRequired();
    }
}
