using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SwapSquare.Common.Services.UserIdentity;

public class UserIdentityService : IUserIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CommonUserInfo? GetUserInfo()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

        if (userId is null || username is null)
        {
            return null;
        }

        return new CommonUserInfo(Guid.Parse(userId), username, email);
    }
}