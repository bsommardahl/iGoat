using System;
using System.Collections.Generic;
using iGoat.Domain.Entities;
using Machine.Specifications;

namespace iGoat.Service.Specs
{
    public class when_getting_details_for_a_specific_delivery_item_that_does_not_exist : given_a_delivery_service_context
    {
        private const int DeliveryItemId = 765;
        private const string AuthKey = "some auth key";

        private Establish context = () => MockProfileService.Setup(
            x => x.GetProfile(Moq.It.Is<string>(y => y == AuthKey)))
                                              .Returns(new Profile
                                                           {
                                                               Items = new List<DeliveryItem>()
                                                           });

        private Because of =
            () => _exception = Catch.Exception(() => Service.GetDeliveryItemDetails(AuthKey, DeliveryItemId));

        private It should_throw_the_expected_exception =
            () => _exception.ShouldContainErrorMessage("Delivery item not found.");

        private static Exception _exception;
    }
}