using iGoat.Domain;
using Machine.Specifications;
using Moq;

namespace iGoat.Service.Specs
{
    public abstract class given_a_delivery_service_context
    {
        protected static IDeliveryWebService Service;
        protected static Mock<IProfileService> MockProfileService;

        private Establish base_context = () =>
                                             {
                                                 MockProfileService = new Mock<IProfileService>();
                                                 Service = new DeliveryWebService(MockProfileService.Object);                                            
                                             };    
    }
}