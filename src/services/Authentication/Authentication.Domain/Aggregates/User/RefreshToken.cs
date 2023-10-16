using System.Security.Cryptography;
using SwapSquare.Authentication.Domain.Common;

namespace SwapSquare.Authentication.Domain.Aggregates.User;

public class RefreshToken : Entity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
    public static string GetRandomTokenString(int length = 32)
    {
        char[] alphabet = [.. "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"];
        byte[] bytes = RandomNumberGenerator.GetBytes(length);
        string token = new(bytes.Select(b => alphabet[b % alphabet.Length]).ToArray());
        return token;
    }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
}