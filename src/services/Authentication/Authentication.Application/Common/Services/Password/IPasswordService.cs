namespace SwapSquare.Authentication.Application.Common.Services.Password;

public interface IPasswordService
{
    (byte[] hash, byte[] salt) GenerateHashAndSalt(string password);
    bool IsValidPassword(string password, byte[] hash, byte[] salt);
}