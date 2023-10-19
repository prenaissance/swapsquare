using Microsoft.Extensions.DependencyInjection;
using SwapSquare.Common.Services.UserIdentity;

namespace SwapSquare.Common.DI;

public static class Web
{
    /// <summary>
    /// Add common services for web projects
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCommonWeb(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserIdentityService, UserIdentityService>();
        return services;
    }
}