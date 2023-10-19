using Microsoft.EntityFrameworkCore;
using SwapSquare.Authentication.Application.Users;
using SwapSquare.Authentication.DataAccess.Persistance;
using SwapSquare.Authentication.Domain.Aggregates.User;
using SwapSquare.Authentication.Domain.Common;

namespace SwapSquare.Authentication.DataAccess.Repositories;

public class UserRepository(AuthDbContext dbContext) : IUserRepository
{
    public IUnitOfWork UnitOfWork => dbContext;
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Users.AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public User Update(User entity, CancellationToken cancellationToken = default)
    {
        var entry = dbContext.Users.Update(entity);
        return entry.Entity;
    }

    public User Delete(User entity, CancellationToken cancellationToken = default)
    {
        var entry = dbContext.Users.Remove(entity);
        return entry.Entity;
    }
}