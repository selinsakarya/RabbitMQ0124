using BankingApi.Events;

namespace EventBus.Bus;

public interface IEventHandler<in TEvent> : IEventHandler where TEvent: Event
{
    Task Handle<T>(T @event);
}

public interface IEventHandler
{
    
}