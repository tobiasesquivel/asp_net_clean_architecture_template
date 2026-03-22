using Cortex.Mediator.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAppMediator();
        return services;
    }

    private static IServiceCollection AddAppMediator(this IServiceCollection services)
    {
        services.AddCortexMediator([typeof(DependencyInjection)],
        opts =>
        {
            opts.AddDefaultBehaviors();
        });
        return services;
    }
}