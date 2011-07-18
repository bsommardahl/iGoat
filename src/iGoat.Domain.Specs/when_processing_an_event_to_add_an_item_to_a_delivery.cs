using System;
using System.Collections.Generic;
using FizzWare.NBuilder;
using iGoat.Domain.Entities;
using Machine.Specifications;
using Moq;
using NCommons.Testing.Equality;
using It = Machine.Specifications.It;

namespace iGoat.Domain.Specs
{
    public class when_processing_an_event_to_add_an_item_to_a_delivery
    {
        private Establish context = () =>
                                        {
                                            _mockProfileRepository = new Mock<IProfileRepository>();
                                            _processor =
                                                new AddDeliveryItemToDeliveryEventProcessor(_mockProfileRepository.Object) as IEventProcessor;

                                            _existingDelivery = new Delivery
                                                               {
                                                                   Id = DeliveryId,
                                                                   CompletedOn = new DateTime(2020, 4, 3),
                                                                   Location = Builder<Location>.CreateNew().Build(),
                                                                   Items = new List<DeliveryItem>(),
                                                               };

                                            _existingDeliveryItem = Builder<DeliveryItem>.CreateNew()
                                                .With(x=>x.Id = DeliveryItemId)
                                                .With(x => x.Status = DeliveryItemStatus.InTruck)
                                                .Build();
                                            
                                            _profileFromRepository = new Profile
                                                                         {
                                                                             Id = ProfileId,
                                                                             Items = new List<DeliveryItem>
                                                                                         {
                                                                                             _existingDeliveryItem
                                                                                         },
                                                                             Deliveries = new List<Delivery>
                                                                                              {
                                                                                                  _existingDelivery
                                                                                              },
                                                                         };

                                            SetupExpectedObjects();

                                            _mockProfileRepository.Setup(
                                                x => x.Get(Moq.It.Is<string>(y => y == AuthKey)))
                                                .Returns(_profileFromRepository);

                                            _mockProfileRepository.Setup(
                                                x => x.Update(Moq.It.Is<Profile>(y => y == _profileFromRepository)))
                                                .Callback((Profile x) => _actualProfilePassedToRepository = x);

                                        };

        private static void SetupExpectedObjects()
        {
            _expectedDelieryItem = new DeliveryItem
                                       {
                                           Id = _existingDeliveryItem.Id,
                                           ItemType = _existingDeliveryItem.ItemType,
                                           Status = DeliveryItemStatus.Delivered,
                                       };

            _expectedDelivery = new Delivery
                                    {
                                        Id = _existingDelivery.Id,
                                        CompletedOn = _existingDelivery.CompletedOn,
                                        Location = _existingDelivery.Location,
                                        Items = new List<DeliveryItem>
                                                    {
                                                        _expectedDelieryItem
                                                    }
                                    };

            _expectedProfileToUpdate = new Profile
                                           {
                                               Id = ProfileId,
                                               Items = new List<DeliveryItem>
                                                           {
                                                               _expectedDelieryItem
                                                           },
                                               Deliveries = new List<Delivery>
                                                                {
                                                                    _expectedDelivery
                                                                }
                                           };
        }

        private Because of = () => _result = _processor.Process(new AddDeliveryItemToDelivery
                                                                    {
                                                                        AuthKey = AuthKey,
                                                                        DeliveryItemId = DeliveryItemId,
                                                                        DeliveryId = DeliveryId
                                                                    });

        private It should_update_the_expected_profile_in_the_repo =
            () => _expectedProfileToUpdate.ToExpectedObject().ShouldEqual(_actualProfilePassedToRepository);

        private static IEventProcessor _processor;
        private static Mock<IProfileRepository> _mockProfileRepository;
        private static Profile _profileFromRepository;
        private static Profile _actualProfilePassedToRepository;
        private static Profile _expectedProfileToUpdate;
        private static Delivery _existingDelivery;
        private static Delivery _expectedDelivery;
        private static DeliveryItem _existingDeliveryItem;
        private static DeliveryItem _expectedDelieryItem;
        private static IProcessEventResponse _result;
        private const string AuthKey = "some auth key";
        private const int ProfileId = 543;
        private const int DeliveryItemId = 876;
        private const int DeliveryId = 7654;
    }
}