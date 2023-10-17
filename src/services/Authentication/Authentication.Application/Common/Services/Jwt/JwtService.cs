namespace SwapSquare.Authentication.Application.Common.Services.Jwt;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SwapSquare.Authentication.Application.Configuration;
using SwapSquare.Authentication.Domain.Aggregates.User;

public class JwtService : IJwtService
{
    private readonly byte[] _key;
    private readonly TimeSpan _tokenLifetime;
    private readonly TimeSpan _refreshTokenLifetime;
    public JwtService(JwtSettings jwtSettings)
    {
        _key = Convert.FromBase64String(jwtSettings.SymmetricKey);
        _tokenLifetime = TimeSpan.FromMinutes(jwtSettings.ExpirationInMinutes);
        _refreshTokenLifetime = TimeSpan.FromMinutes(jwtSettings.RefreshTokenExpirationInDays);
    }
    public string GenerateTokenForUser(User user) => GenerateTokenForUser(user, "swap-square");
    public string GenerateTokenForUser(User user, string audience)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_tokenLifetime),
            Issuer = "swap-square",
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}