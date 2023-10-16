using Microsoft.EntityFrameworkCore;
using SwapSquare.Authentication.Domain.Aggregates.User;
using SwapSquare.Authentication.Domain.Common;

namespace SwapSquare.Authentication.DataAccess.Persistance;

public class AuthDbContext : DbContext, IUnitOfWork
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }
}