using AutoMapper;
using iGoat.Domain;
using Machine.Specifications;
using Moq;

namespace iGoat.Service.Specs
{
    public abstract class given_a_delivery_service_context
    {
        protected static IDeliveryWebService Service;
        protected static Mock<IProfileService> MockProfileService;
        protected static Mock<IEventProcessorFactory> MockEventProcessorFactory;
        protected static Mock<IMappingEngine> MockMappingEngine;

        private Establish base_context = () =>
                                             {
                                                 MockEventProcessorFactory = new Mock<IEventProcessorFactory>();
                                                 MockProfileService = new Mock<IProfileService>();
                                                 MockMappingEngine = new Mock<IMappingEngine>();
                                                 Service = new DeliveryWebService(MockProfileService.Object,
                                                                                  MockEventProcessorFactory.Object,
                                                                                  MockMappingEngine.Object);
                                             };
    }
}