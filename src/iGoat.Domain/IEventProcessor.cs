namespace iGoat.Domain
{
    public interface IEventProcessor
    {
        IProcessEventResponse Process(IEvent @event);
    }
}