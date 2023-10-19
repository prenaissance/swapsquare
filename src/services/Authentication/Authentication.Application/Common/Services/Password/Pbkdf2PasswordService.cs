using System.Security.Cryptography;

namespace SwapSquare.Authentication.Application.Common.Services.Password;

public sealed class Pbkdf2PasswordService : IPasswordService
{
    private const int keySize = 64;
    private const int iterations = 10000;
    public (byte[] hash, byte[] salt) GenerateHashAndSalt(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password: password,
            salt: salt,
            iterations: iterations,
            hashAlgorithm: HashAlgorithmName.SHA512,
            outputLength: keySize
        );
        return (hash, salt);
    }

    public bool IsValidPassword(string password, byte[] hash, byte[] salt)
    {
        var newHash = Rfc2898DeriveBytes.Pbkdf2(
            password: password,
            salt: salt,
            iterations: iterations,
            hashAlgorithm: HashAlgorithmName.SHA512,
            outputLength: keySize
        );
        return newHash.SequenceEqual(hash);
    }
}