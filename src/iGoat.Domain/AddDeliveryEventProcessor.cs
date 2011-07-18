using System;
using System.Linq;
using iGoat.Domain.Entities;

namespace iGoat.Domain
{
    public class AddDeliveryEventProcessor : IEventProcessor
    {
        private readonly IProfileRepository _profileRepository;

        public AddDeliveryEventProcessor(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public IProcessEventResponse Process(IEvent @event)
        {
            var profile = _profileRepository.Get(@event.AuthKey);

            var deliveryRequest = @event as NewDeliveryEvent;

            var newDelivery = new Delivery
                                  {
                                      CompletedOn = deliveryRequest.CompletedOn,
                                      Location = deliveryRequest.Location,
                                  };

            profile.Deliveries.Add(newDelivery);

            var updatedProfile = _profileRepository.Update(profile);
            var updatedDelivery = updatedProfile.Deliveries.Last();

            return new DeliveryAddedResponse
                       {
                           DeliveryId = updatedDelivery.Id,
                       };
        }
    }
}