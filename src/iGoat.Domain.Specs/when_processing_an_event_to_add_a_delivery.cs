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
    public class when_processing_an_event_to_add_a_delivery
    {
        private const string AuthKey = "some auth key";
        private const int ProfileId = 543;
        private const int DeliveryId = 9876;
        private static AddDeliveryEventProcessor _processor;
        private static IProcessEventResponse _result;
        private static NewDeliveryEvent _newDeliveryEvent;
        private static Mock<IProfileRepository> _mockProfileRepository;
        private static Profile _profileFromRepository;
        private static Profile _actualProfilePassedToRepository;
        private static Profile _expectedProfileToUpdate;
        private static Delivery _newDelivery;
        private static Profile _updatedProfile;
        private static Delivery _newDeliveryWithId;
        private static DeliveryAddedResponse _expectedResult;

        private Establish context = () =>
                                        {
                                            _newDeliveryEvent = new NewDeliveryEvent
                                                                    {
                                                                        CompletedOn = new DateTime(2010, 4, 3),
                                                                        Location = Builder<Location>.CreateNew().Build(),
                                                                        AuthKey = AuthKey,
                                                                    };

                                            _newDelivery = new Delivery
                                                               {
                                                                   CompletedOn = _newDeliveryEvent.CompletedOn,
                                                                   Location = _newDeliveryEvent.Location,
                                                               };

                                            _profileFromRepository = new Profile
                                                                         {
                                                                             Id = ProfileId,
                                                                             Deliveries = new List<Delivery>(),
                                                                         };

                                            _mockProfileRepository = new Mock<IProfileRepository>();
                                            _processor = new AddDeliveryEventProcessor(_mockProfileRepository.Object);

                                            _mockProfileRepository
                                                .Setup(x => x.Get(Moq.It.Is<string>(y => y == AuthKey)))
                                                .Returns(_profileFromRepository);

                                            _newDeliveryWithId = new Delivery
                                                                     {
                                                                         Id = DeliveryId,
                                                                         CompletedOn = _newDeliveryEvent.CompletedOn,
                                                                         Location = _newDeliveryEvent.Location,
                                                                     };

                                            _updatedProfile = new Profile
                                                                  {
                                                                      Id = ProfileId,
                                                                      Deliveries = new List<Delivery>
                                                                                       {
                                                                                           _newDeliveryWithId
                                                                                       }
                                                                  };

                                            _mockProfileRepository
                                                .Setup(
                                                    x => x.Update(Moq.It.Is<Profile>(y => y == _profileFromRepository)))
                                                .Returns(_updatedProfile)
                                                .Callback((Profile x) => _actualProfilePassedToRepository = x)
                                                ;

                                            _expectedProfileToUpdate = new Profile
                                                                           {
                                                                               Id = ProfileId,
                                                                               Deliveries = new List<Delivery>
                                                                                                {
                                                                                                    _newDelivery
                                                                                                }
                                                                           };

                                            _expectedResult = new DeliveryAddedResponse
                                                                  {
                                                                      DeliveryId = DeliveryId,
                                                                  };
                                        };

        private Because of = () => _result = _processor.Process(_newDeliveryEvent);

        private It should_return_a_successful_response = () => 
            _expectedResult.ToExpectedObject().ShouldEqual(_result);

        private It should_update_the_profile_with_the_new_delivery = () => 
            _expectedProfileToUpdate.ToExpectedObject().ShouldEqual(_actualProfilePassedToRepository);
    }
}