namespace SwapSquare.Authentication.Application.Common.Services.Jwt;

public record TokenPairResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}