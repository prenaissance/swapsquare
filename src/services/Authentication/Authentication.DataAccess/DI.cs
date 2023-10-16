using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwapSquare.Authentication.Application.Users;
using SwapSquare.Authentication.DataAccess.Persistance;
using SwapSquare.Authentication.DataAccess.Repositories;
using SwapSquare.Authentication.Domain.Common;

namespace SwapSquare.Authentication.DataAccess;

public static class DI
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"));
        });

        services.AddScoped<IUnitOfWork, AuthDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
