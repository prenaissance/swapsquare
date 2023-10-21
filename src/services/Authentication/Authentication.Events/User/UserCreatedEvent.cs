namespace SwapSquare.Authentication.Events.User;

public record UserCreatedEvent(
    Guid UserId,
    string? Email,
    string Username
);