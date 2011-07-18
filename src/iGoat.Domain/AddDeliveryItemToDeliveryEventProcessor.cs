using System;
using System.Linq;

namespace iGoat.Domain
{
    public class AddDeliveryItemToDeliveryEventProcessor : IEventProcessor
    {
        private readonly IProfileRepository _profileRepository;

        public AddDeliveryItemToDeliveryEventProcessor(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public IProcessEventResponse Process(IEvent @event)
        {
            var profile = _profileRepository.Get(@event.AuthKey);

            var req = @event as AddDeliveryItemToDelivery;

            var delivery = profile.Deliveries.SingleOrDefault(x => x.Id == req.DeliveryId);
            var deliveryItem = profile.Items.SingleOrDefault(x => x.Id == req.DeliveryItemId);

            deliveryItem.Status = DeliveryItemStatus.Delivered;
            delivery.Items.Add(deliveryItem);

            _profileRepository.Update(profile);

            return null;
        }
    }
}