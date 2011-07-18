using System;
using iGoat.Service.Contracts;

namespace iGoat.Service
{
    public class ProcessAddItemToDeliveryEventRequest : IProcessEventRequest
    {
        public int DeliveryItemId { get; set; }

        public int DeliveryId { get; set; }
    }
}