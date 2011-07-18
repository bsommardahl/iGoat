using System;

namespace iGoat.Domain
{
    public class AddDeliveryItemToDelivery : IEvent
    {
        public string AuthKey
        {
            get; set;
        }

        public int DeliveryItemId { get; set; }

        public int DeliveryId { get; set; }
    }
}