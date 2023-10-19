using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserEntity = SwapSquare.Authentication.Domain.Aggregates.User.User;

namespace SwapSquare.Authentication.DataAccess.Persistance.Configuration.User;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Username).HasMaxLength(256);
        builder.HasIndex(x => x.Username)
            .IsUnique();
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.HasIndex(x => x.Email)
            .IsUnique()
            .AreNullsDistinct(false);
        builder.Property(x => x.PasswordHash).HasMaxLength(256);
        builder.Property(x => x.PasswordSalt).HasMaxLength(256);
        builder.HasMany(x => x.RefreshTokens).WithOne().HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // make sure _refreshTokens is mapped
        builder.Metadata.FindNavigation(nameof(UserEntity.RefreshTokens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}