using SwapSquare.Authentication.Domain.Aggregates.User;

namespace SwapSquare.Authentication.Application.Common.Services.Jwt;

public interface IJwtService
{
    TokenPairResponse GenerateTokensForUser(User user);
    TokenPairResponse GenerateTokensForUser(User user, string audience);
}