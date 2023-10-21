using MediatR;
using Microsoft.EntityFrameworkCore;
using SwapSquare.Authentication.Domain.Aggregates.User;
using SwapSquare.Authentication.Domain.Common;

namespace SwapSquare.Authentication.DataAccess.Persistance;

public class AuthDbContext(
    DbContextOptions<AuthDbContext> options,
    IMediator mediator
    ) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);
        var events = ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToArray();
        return result;
    }
}