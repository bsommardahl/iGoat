using iGoat.Domain;
using iGoat.Service.Contracts;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace iGoat.Service.Specs
{    
    public class when_processing_an_event : given_a_delivery_service_context
    {
        private static IProcessEventRequest _eventRequestFromClient;
        private static Mock<IEventProcessor> _processor;
        private static IEvent _eventForProcessor;

        private Establish context = () =>
                                        {
                                            _eventRequestFromClient = new Mock<IProcessEventRequest>().Object;

                                            _eventForProcessor = new Mock<IEvent>().Object;

                                            MockMappingEngine.Setup(x =>
                                                                    x.Map<IProcessEventRequest, IEvent>(
                                                                        Moq.It.Is<IProcessEventRequest>(
                                                                            y => y == _eventRequestFromClient)))
                                                .Returns(_eventForProcessor);

                                            _processor = new Mock<IEventProcessor>();

                                            MockEventProcessorFactory.Setup(x => x.Create(Moq.It.IsAny<IEvent>()))
                                                .Returns(_processor.Object);
                                        };

        private Because of = () => Service.ProcessEvent(_eventRequestFromClient);

        private It should_process_the_correct_event =
            () => _processor.Verify(x => x.Process(Moq.It.Is<IEvent>(y => y == _eventForProcessor)));        
    }
}