using System.Text;
using System.Text.Json;
using BankingApi.Commands;
using BankingApi.Events;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BankingApi.Bus;

public class RabbitMqBus : IEventBus
{
    private readonly IMediator _mediator;

    private readonly Dictionary<string, List<Type>> _handlers;

    private readonly List<Type> _eventTypes;

    public RabbitMqBus(IMediator mediator)
    {
        _mediator = mediator;
        _handlers = new Dictionary<string, List<Type>>();
        _eventTypes = new List<Type>();
    }

    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public void Publish<T>(T @event) where T : Event
    {
        ConnectionFactory factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672/")
        };

        using IConnection connection = factory.CreateConnection();
        using IModel channel = connection.CreateModel();
        
        string eventName = @event.GetType().Name;

        channel.QueueDeclare(eventName, false, false, false, null);

        string message = JsonSerializer.Serialize(@event);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty, routingKey: eventName, basicProperties: null, body: body);
    }

    public void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
    {
        string eventName = typeof(T).Name;

        Type handlerType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }

        if (!_handlers.ContainsKey(eventName))
        {
            _handlers.Add(eventName, new List<Type>());
        }

        if (_handlers[eventName].Any(h => h == handlerType))
        {
            throw new ArgumentException($"Handler type {handlerType.Name} already is registered for {eventName}");
        }

        StartBasicConsume<T>();
    }

    private void StartBasicConsume<T>() where T : Event
    {
        ConnectionFactory factory = new ConnectionFactory
        {
            Uri = new Uri("amqp://guest:guest@localhost:5672/"),
            DispatchConsumersAsync = true
        };

        using IConnection connection = factory.CreateConnection();
        using IModel channel = connection.CreateModel();
        
        string eventName = typeof(T).Name;

        channel.QueueDeclare(eventName, false, false, false, null);

        AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += ConsumeReceived;

        channel.BasicConsume(eventName, true, consumer);
    }

    private async Task ConsumeReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        string? eventName = eventArgs.RoutingKey;

        string message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (_handlers.TryGetValue(eventName, out List<Type>? subscriptions))
        {
            foreach (var subscription in subscriptions)
            {
                object? handler = Activator.CreateInstance(subscription);

                if (handler == null)
                {
                    continue;
                }

                Type? eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);

                if (eventType == null)
                {
                    continue;
                }

                object? @event = JsonSerializer.Deserialize(message, eventType);

                Type concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                await (Task)concreteType.GetMethod("Handle")!.Invoke(handler, [@event])!;
            }
        }
    }
}