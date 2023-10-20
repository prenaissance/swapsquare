using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace SwapSquare.Common.Extensions;

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
                                Id = "Bearer",
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