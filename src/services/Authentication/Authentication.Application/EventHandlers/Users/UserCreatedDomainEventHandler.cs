using MassTransit;
using MediatR;
using SwapSquare.Authentication.Domain.Aggregates.User.Events;
using SwapSquare.Authentication.Events.User;

namespace SwapSquare.Authentication.Application.EventHandlers.Users;

public class UserCreatedDomainEventHandler(IBus bus) : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await bus.Publish(new UserCreatedEvent(
            notification.UserId,
            notification.Email,
            notification.Username
        ), cancellationToken);
    }
}