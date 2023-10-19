using SwapSquare.Authentication.Domain.Aggregates.User.Exceptions;
using SwapSquare.Authentication.Domain.Common;

namespace SwapSquare.Authentication.Domain.Aggregates.User;

public class User : AggregateRoot
{
    public string Username { get; set; } = null!;
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    private readonly List<RefreshToken> _refreshTokens = [];
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    public string? Email { get; set; }
    public bool HasCredentialsAuthentication() => PasswordHash is not null && PasswordSalt is not null;

    public RefreshToken CreateRefreshToken(int daysToExpire = 7)
    {
        var refreshToken = new RefreshToken
        {
            UserId = Id,
            Token = RefreshToken.GetRandomTokenString(),
            ExpiresAt = DateTime.UtcNow.AddDays(daysToExpire),
            CreatedAt = DateTime.UtcNow,
        };
        _refreshTokens.Add(refreshToken);
        return refreshToken;
    }
    public void RevokeRefreshToken(string token, string ipAddress)
    {
        var refreshToken = _refreshTokens.SingleOrDefault(x => x.Token == token);
        if (refreshToken is null) return;
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
    }
    public RefreshToken ReplaceRefreshToken(string token, string ipAddress, int daysToExpire = 7)
    {
        var refreshToken = _refreshTokens
            .Where(r => r.RevokedAt is null)
            .SingleOrDefault(x => x.Token == token);
        if (refreshToken is null)
        {
            throw new RefreshTokenInvalidException("No active refresh token found for the given token");
        }

        var newRefreshToken = CreateRefreshToken(daysToExpire);
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        return newRefreshToken;
    }
}