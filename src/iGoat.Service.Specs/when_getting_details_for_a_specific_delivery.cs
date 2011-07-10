using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using iGoat.Domain.Entities;
using iGoat.Service.Contracts;
using Machine.Specifications;
using NCommons.Testing.Equality;

namespace iGoat.Service.Specs
{
    public class when_getting_details_for_a_specific_delivery : given_a_delivery_service_context
    {
        private const int DeliveryId = 654;
        private const string AuthKey = "some auth key";
        private static DeliveryDetails _result;
        private static DeliveryDetails _expectedDeliveryItem;

        private Establish context = () =>
                                        {
                                            var delivery = new Delivery
                                                               {
                                                                   Id = DeliveryId,
                                                                   CompletedOn = new DateTime(2010, 4, 5),
                                                                   Items = Builder<DeliveryItem>.CreateListOfSize(10)
                                                                       .WhereAll()
                                                                       .Have(x => x.ItemType = new DeliveryItemType
                                                                                                   {
                                                                                                       Name =
                                                                                                           "something",
                                                                                                   })
                                                                       .Build(),
                                                                   Location = new Location
                                                                                  {
                                                                                      Id = 987,
                                                                                      Name = "some name",
                                                                                      Latitude = 85.5483m,
                                                                                      Longitue = -36.432m,
                                                                                  }
                                                               };

                                            MockProfileService.Setup(
                                                x => x.GetProfile(Moq.It.Is<string>(y => y == AuthKey)))
                                                .Returns(new Profile
                                                             {
                                                                 Deliveries = new List<Delivery>
                                                                                  {
                                                                                      delivery
                                                                                  }
                                                             });

                                            _expectedDeliveryItem = new DeliveryDetails
                                                                        {
                                                                            Id = delivery.Id,
                                                                            CompletedOn = delivery.CompletedOn,
                                                                            Items =
                                                                                delivery.Items.Select(
                                                                                    x => new DeliveryItemSummary
                                                                                             {
                                                                                                 Id = x.Id,
                                                                                                 Type =
                                                                                                     x.ItemType.ToString
                                                                                                     ()
                                                                                             }).ToList(),
                                                                            Location = new LocationData
                                                                                           {
                                                                                               Name =
                                                                                                   delivery.Location.
                                                                                                   Name,
                                                                                               Latitude =
                                                                                                   delivery.Location.
                                                                                                   Latitude,
                                                                                               Longitude =
                                                                                                   delivery.Location.
                                                                                                   Longitue,
                                                                                           }
                                                                        };
                                        };

        private Because of = () => _result = Service.GetDeliveryDetails(AuthKey, DeliveryId);

        private It should_return_the_expected_delivery_item =
            () => _expectedDeliveryItem.ToExpectedObject().ShouldEqual(_result);
    }
}