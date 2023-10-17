using SwapSquare.Authentication.Domain.Aggregates.User;

namespace SwapSquare.Authentication.Application.Common.Services.Jwt;

public interface IJwtService
{
    string GenerateTokenForUser(User user);
    string GenerateTokenForUser(User user, string audience);
}