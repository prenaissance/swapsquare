namespace SwapSquare.Authentication.Domain.Aggregates.User.Exceptions;

public class RefreshTokenInvalidException(string message) : Exception(message);