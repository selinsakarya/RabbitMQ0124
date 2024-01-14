using BankingApi.Events;

namespace EventBus.Commands;

public abstract class Command : Message
{
    public DateTime TimeStamp { get; protected set; }

    protected Command()
    {
        TimeStamp = DateTime.UtcNow;
    }
}