namespace SwapSquare.Common.DI;

using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwapSquare.Common.Settings;

public static class MassTransit
{
    private static Action<BinderOptions> binderOptionsFactory = options =>
    {
        options.BindNonPublicProperties = false;
        options.ErrorOnUnknownConfiguration = true;
    };
    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, rabbitMqConfig) =>
            {
                var configuration = ctx.GetRequiredService<IConfiguration>();

                var serviceSettings = configuration
                    .GetRequiredSection(nameof(ServiceSettings))
                    .Get<ServiceSettings>(binderOptionsFactory)!;
                var rabbitMqSettings = configuration
                    .GetRequiredSection(nameof(RabbitMqSettings))
                    .Get<RabbitMqSettings>(binderOptionsFactory)!;


                rabbitMqConfig.Host(rabbitMqSettings.Host);
                rabbitMqConfig.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName));
            });
        });
        return services;
    }
}