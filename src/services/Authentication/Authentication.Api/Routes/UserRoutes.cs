using SwapSquare.Authentication.Api.Extensions;
using SwapSquare.Authentication.Application.Users;
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
        return TypedResults.Ok(userIdentityService.GetUserInfo());
    }
}