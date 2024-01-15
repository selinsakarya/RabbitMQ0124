using EventBus.Bus;
using MicroRabbit.Domain.Core.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public static class DependencyContainer
{
    public static void AddRabbitMqEventBus(this IServiceCollection services)
    {
        services.AddTransient<IEventBus, RabbitMqBus>();
    }
}