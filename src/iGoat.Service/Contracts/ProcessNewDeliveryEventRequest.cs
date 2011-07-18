using System;

namespace iGoat.Service.Contracts
{
    public class ProcessNewDeliveryEventRequest : IProcessEventRequest
    {
        public DateTime CompletedOn { get; set; }

        public string Name { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }
    }
}