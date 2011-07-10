using System.Collections.Generic;
using FizzWare.NBuilder;
using iGoat.Domain.Entities;
using iGoat.Service.Contracts;
using Machine.Specifications;
using NCommons.Testing.Equality;

namespace iGoat.Service.Specs
{
    public class when_getting_details_for_a_specific_delivery_item : given_a_delivery_service_context
    {
        private const int DeliveryItemId = 765;
        private const string AuthKey = "some auth key";

        private static DeliveryItemDetails _result;
        private static DeliveryItemDetails _expectedDeliveryItem;
        private static DeliveryItem _deliveryItem;

        private Establish context = () =>
                                        {
                                            _deliveryItem = Builder<DeliveryItem>.CreateNew()
                                                .With(x => x.ItemType = new DeliveryItemType {Name = "something"})
                                                .And(x => x.Id = DeliveryItemId)
                                                .Build();

                                            MockProfileService.Setup(
                                                x => x.GetProfile(Moq.It.Is<string>(y => y == AuthKey)))
                                                .Returns(new Profile
                                                             {
                                                                 Items = new List<DeliveryItem>
                                                                             {
                                                                                 _deliveryItem,
                                                                             }
                                                             });

                                            _expectedDeliveryItem = new DeliveryItemDetails
                                                                        {
                                                                            Id = _deliveryItem.Id,
                                                                            Type = _deliveryItem.ItemType.Name,
                                                                            Status = _deliveryItem.Status.ToString(),
                                                                        };
                                        };

        private Because of = () => _result = Service.GetDeliveryItemDetails(AuthKey, DeliveryItemId);

        private It should_return_the_expected_delivery_item_details =
            () => _expectedDeliveryItem.ToExpectedObject().ShouldEqual(_result);
    }
}