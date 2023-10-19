using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwapSquare.Authentication.Application.Common.Services.Jwt;
using SwapSquare.Authentication.Application.Common.Services.Password;
using SwapSquare.Authentication.Application.Configuration;

namespace SwapSquare.Authentication.Application;

public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddSingleton<IPasswordService, Pbkdf2PasswordService>();
        services.AddSingleton<IJwtService, JwtService>();

        return services;
    }
}