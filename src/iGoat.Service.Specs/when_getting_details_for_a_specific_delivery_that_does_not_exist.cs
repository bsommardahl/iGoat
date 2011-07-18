using System;
using System.Collections.Generic;
using iGoat.Domain.Entities;
using Machine.Specifications;

namespace iGoat.Service.Specs
{
    public class when_getting_details_for_a_specific_delivery_that_does_not_exist : given_a_delivery_service_context
    {
        private const int DeliveryId = 654;
        private const string AuthKey = "some auth key";
        
        private Establish context = () => MockProfileService.Setup(
            x => x.GetProfile(Moq.It.Is<string>(y => y == AuthKey)))
                                              .Returns(new Profile
                                                           {
                                                               Deliveries = new List<Delivery>()
                                                           });

        private Because of = () => _exception = Catch.Exception(() => Service.GetDeliveryDetails(AuthKey, DeliveryId));

        private It should_throw_the_expected_exception =
            () => _exception.ShouldContainErrorMessage("Delivery not found.");

        private static Exception _exception;
    }
}