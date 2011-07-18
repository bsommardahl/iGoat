using System;
using StructureMap;

namespace iGoat.Domain
{
    public class EventProcessorFactory : IEventProcessorFactory
    {
        private readonly IContainer _container;

        public EventProcessorFactory(IContainer container)
        {
            _container = container;
        }

        #region IEventProcessorFactory Members

        public IEventProcessor Create(IEvent @event)
        {
            return _container.GetInstance<IEventProcessor>(@event.ToString());
        }

        #endregion
    }
}