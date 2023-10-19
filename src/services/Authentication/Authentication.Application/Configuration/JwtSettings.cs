namespace SwapSquare.Authentication.Application.Configuration;

public record JwtSettings
{
    public required string PrivateKey { get; init; }
    public required string PublicKey { get; init; }
    public int ExpirationInMinutes { get; init; } = 15;
    public int RefreshTokenExpirationInDays { get; init; } = 30;
}