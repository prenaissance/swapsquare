using SwapSquare.Authentication.Domain.Aggregates.User;
using SwapSquare.Authentication.Domain.Common;

namespace SwapSquare.Authentication.Application.Users;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}