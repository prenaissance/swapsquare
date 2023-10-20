using SwapSquare.Authentication.Application.Users;
using SwapSquare.Authentication.Application.Users.Dtos;
using SwapSquare.Common.Extensions;
using SwapSquare.Common.Services.UserIdentity;

namespace SwapSquare.Authentication.Api.Routes;

public static class UserRoutes
{
    public const string basePath = "/users";
    public static IEndpointRouteBuilder MapUserRoutes(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(basePath);
        group.MapGet("", GetUserInfo).WithAuthorization();
        return endpoints;
    }

    public static async Task<IResult> GetUserInfo(
        IUserIdentityService userIdentityService,
        IUserRepository userRepository)
    {
        Guid userId = userIdentityService.GetUserInfo()!.Id;
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(GetUserDto.FromUser(user));
    }
}