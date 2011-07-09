using System;
using System.Collections.Generic;

namespace iGoat.Service
{
    public class DeliveryData
    {
        public int Id { get; set; }

        public List<DeliveryItemSummary> Items { get; set; }

        public DateTime CompletedOn { get; set; }

        public LocationData Location { get; set; }
    }
}