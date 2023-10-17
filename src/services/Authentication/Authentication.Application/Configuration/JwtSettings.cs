namespace SwapSquare.Authentication.Application.Configuration;

public record JwtSettings
{
    public required string SymmetricKey { get; init; }
    public int ExpirationInMinutes { get; init; } = 15;
    public int RefreshTokenExpirationInDays { get; init; } = 30;
}