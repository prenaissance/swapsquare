using MediatR;

namespace SwapSquare.Authentication.Domain.Aggregates.User.Events;

public record UserCreatedDomainEvent(
    Guid UserId,
    string? Email,
    string Username
) : INotification;