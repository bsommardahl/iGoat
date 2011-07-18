using Machine.Specifications;
using Moq;
using NCommons.Testing.Equality;
using StructureMap;
using It = Machine.Specifications.It;

namespace iGoat.Domain.Specs
{
    public class when_creating_an_event_processor_from_an_event
    {
        private static IEventProcessorFactory _factory;
        private static IEvent _event;
        private static IEventProcessor _result;
        private static IEventProcessor _expectedProcessor;
        private static IContainer _container;

        private Establish context = () =>
                                        {
                                            _container = new Container();
                                            _factory = new EventProcessorFactory(_container);

                                            _expectedProcessor = new Mock<IEventProcessor>().Object;
                                            var unExpectedEventProcessor = new Mock<IEventProcessor>().Object;

                                            _event = new Mock<IEvent>().Object;

                                            _container.Configure(x =>
                                                                     {
                                                                         x.For<IEventProcessor>()
                                                                             .Use(unExpectedEventProcessor)
                                                                             .Named("something else"); 
                                                                         
                                                                         x.For<IEventProcessor>()
                                                                             .Use(_expectedProcessor)
                                                                             .Named(_event.ToString());
                                                                     });
                                        };

        private Because of = () => _result = _factory.Create(_event);

        private It should_return_the_expected_event_processor =
            () => _expectedProcessor.ToExpectedObject().ShouldEqual(_result);
    }
}