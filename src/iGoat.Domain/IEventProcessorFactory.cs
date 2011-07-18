namespace iGoat.Domain
{
    public interface IEventProcessorFactory
    {
        IEventProcessor Create(IEvent @event);
    }
}