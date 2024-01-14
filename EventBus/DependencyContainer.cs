using EventBus.Bus;
using EventBus.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus;

public static class DependencyContainer
{
    public static void AddRabbitMqEventBus(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Command).Assembly));
        
        services.AddTransient<IEventBus, RabbitMqBus>();
    }
}