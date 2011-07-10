using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace iGoat.Service.Contracts
{
    [DataContract]
    public class DeliveryDetails
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public List<DeliveryItemSummary> Items { get; set; }

        [DataMember]
        public DateTime CompletedOn { get; set; }

        [DataMember]
        public LocationData Location { get; set; }
    }
}