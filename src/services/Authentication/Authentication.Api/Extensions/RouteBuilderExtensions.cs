using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace SwapSquare.Authentication.Api.Extensions;

public static class RouteBuilderExtensions
{
    public static IEndpointConventionBuilder WithAuthorization(this IEndpointConventionBuilder builder)
    {
        return builder
            .RequireAuthorization()
            .WithOpenApi(options =>
            {
                // require bearer auth
                options.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                return options;
            });
    }
}