namespace SwapSquare.Authentication.Application.Authentication.Dtos;

public record SignInCredentialsDto(
    string Username,
    string Password);