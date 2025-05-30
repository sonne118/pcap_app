using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace outbox;

public static class ServiceExtensions
{
    public static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services.AddSingleton< ISerializer, Serializer>();
        
        services.AddScoped<IOutbox, Outbox>();
        services.AddScoped<IRelay, Relay>();
      

        services.AddHostedService<PublishOutboxJob>();
        services.AddHostedService<CleanupOutboxJob>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}