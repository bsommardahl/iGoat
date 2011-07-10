using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using iGoat.Domain.Entities;
using iGoat.Service.Contracts;
using Machine.Specifications;
using NCommons.Testing.Equality;

namespace iGoat.Service.Specs
{
    public class when_getting_item_summaries_for_truck : given_a_delivery_service_context
    {
        private const string AuthKey = "some auth key";
        private static IList<DeliveryItemSummary> _expectedListOfItemSummaries;
        private static List<DeliveryItemSummary> _result;

        private Establish context = () =>
                                        {
                                            var deliveryItems = Builder<DeliveryItem>.CreateListOfSize(3)
                                                .WhereAll()
                                                .Have(x => x.Status == Domain.DeliveryItemStatus.InTruck)
                                                .And(x => x.ItemType = new DeliveryItemType
                                                {
                                                    Name = "nothing",
                                                })
                                                .WhereTheLast(1)
                                                .Has(x => x.Status == Domain.DeliveryItemStatus.Assigned)
                                                .And(x=>x.ItemType = new DeliveryItemType
                                                                         {
                                                                             Name = "something",
                                                                         })
                                                .Build();

                                            MockProfileService.Setup(
                                                x => x.GetProfile(Moq.It.Is<string>(y => y == AuthKey)))
                                                .Returns(new Profile
                                                             {
                                                                 Id = 1,
                                                                 Items = deliveryItems,
                                                             });

                                            _expectedListOfItemSummaries = deliveryItems
                                                .Where(x => x.Status == Domain.DeliveryItemStatus.InTruck)
                                                .Select(x => new DeliveryItemSummary
                                                                 {
                                                                     Id = x.Id,
                                                                     Type = "something",
                                                                 }).ToList();
                                        };

        private Because of = () => _result = Service.GetMyItems(AuthKey, DeliveryItemStatus.InTruck);

        private It should_return_the_expected_list_of_items =
            () => _expectedListOfItemSummaries.ToExpectedObject().ShouldEqual(_result);
    }
}