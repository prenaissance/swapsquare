using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SwapSquare.Authentication.Application.Configuration;
using SwapSquare.Authentication.Domain.Aggregates.User;

namespace SwapSquare.Authentication.Application.Common.Services.Jwt;

public sealed class JwtService : IJwtService, IDisposable
{
    private readonly RSA _rsa;
    private readonly TimeSpan _tokenLifetime;
    private readonly TimeSpan _refreshTokenLifetime;
    public JwtService(IOptions<JwtSettings> jwtSettingsOptions)
    {
        JwtSettings jwtSettings = jwtSettingsOptions.Value;
        _rsa = RSA.Create();
        _rsa.ImportFromPem(jwtSettings.PrivateKey);
        _tokenLifetime = TimeSpan.FromMinutes(jwtSettings.ExpirationInMinutes);
        _refreshTokenLifetime = TimeSpan.FromMinutes(jwtSettings.RefreshTokenExpirationInDays);
    }
    public TokenPairResponse GenerateTokensForUser(User user) => GenerateTokensForUser(user, "swap-square");
    public TokenPairResponse GenerateTokensForUser(User user, string audience)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_tokenLifetime),
            Issuer = "swap-square",
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new RsaSecurityKey(_rsa),
                SecurityAlgorithms.RsaSha512Signature)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            },
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        string accessToken = tokenHandler.WriteToken(token);
        string refreshToken = user.CreateRefreshToken().Token;
        return new TokenPairResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }

    public void Dispose()
    {
        _rsa.Dispose();
    }
}