namespace SwapSquare.Common.DI;

using System.Reflection;
using global::MassTransit;
using Microsoft.Extensions.DependencyInjection;

public static class MassTransit
{
    public static IServiceCollection AddMassTransitWithRabbitMq(
        this IServiceCollection services,
        string serviceName,
        string rabbitMqHost,
        Assembly assembly = default!)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(assembly ?? Assembly.GetExecutingAssembly());
            x.UsingRabbitMq((ctx, rabbitMqConfig) =>
            {
                rabbitMqConfig.Host(rabbitMqHost);
                rabbitMqConfig.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceName));
            });
        });
        return services;
    }
}