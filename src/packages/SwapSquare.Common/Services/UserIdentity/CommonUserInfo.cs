namespace SwapSquare.Common.Services.UserIdentity;

public record CommonUserInfo(
    Guid Id,
    string Username,
    string? Email
);