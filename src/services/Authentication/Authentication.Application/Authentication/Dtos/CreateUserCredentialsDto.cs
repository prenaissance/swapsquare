namespace SwapSquare.Authentication.Application.Authentication.Dtos;

public record CreateUserCredentialsDto(
    string Username,
    string Email,
    string Password);