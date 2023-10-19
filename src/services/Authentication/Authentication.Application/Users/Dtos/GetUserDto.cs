using SwapSquare.Authentication.Domain.Aggregates.User;

namespace SwapSquare.Authentication.Application.Users.Dtos;

public record GetUserDto(
    Guid Id,
    string Username,
    string? Email
)
{
    public static GetUserDto FromUser(User user)
    {
        return new GetUserDto(
            user.Id,
            user.Username,
            user.Email
        );
    }
}