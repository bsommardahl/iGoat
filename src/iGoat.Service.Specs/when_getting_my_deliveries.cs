using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using iGoat.Domain.Entities;
using Machine.Specifications;
using NCommons.Testing.Equality;

namespace iGoat.Service.Specs
{
    public class when_getting_my_deliveries : given_a_delivery_service_context
    {
        private const string AuthKey = "some auth key";
        private static List<DeliverySummary> _result;
        private static List<DeliverySummary> _expectedDeliveries;
        private static IList<Delivery> _deliveries;

        private Establish context = () =>
                                        {
                                            _deliveries = Builder<Delivery>.CreateListOfSize(10)
                                                .WhereAll()
                                                .Have(x => x.Items = Builder<DeliveryItem>.CreateListOfSize(5).Build())
                                                .Build();

                                            MockProfileService
                                                .Setup(x => x.GetProfile(Moq.It.Is<string>(y => y == AuthKey)))
                                                .Returns(new Profile
                                                             {
                                                                 Deliveries = _deliveries,
                                                             });

                                            _expectedDeliveries =
                                                _deliveries.Select(x => new DeliverySummary
                                                                            {
                                                                                Id = x.Id,
                                                                                CompletedOn = x.CompletedOn,
                                                                                ItemCount = x.Items.Count()
                                                                            }).ToList();
                                        };

        private Because of = () => _result = Service.GetMyDeliveries(AuthKey);

        private It should_return_a_list_of_delivery_summaries =
            () => _expectedDeliveries.ToExpectedObject().ShouldEqual(_result);
    }
}