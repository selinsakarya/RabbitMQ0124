using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Bus;

public interface IEventHandler<in TEvent> : IEventHandler where TEvent: Event
{
    Task Handle<T>(T @event);
}

public interface IEventHandler
{
    
}