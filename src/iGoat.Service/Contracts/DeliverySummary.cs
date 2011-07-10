using System;
using System.Runtime.Serialization;

namespace iGoat.Service.Contracts
{
    [DataContract]
    public class DeliverySummary
    {
        [DataMember]
        public int ItemCount { get; set; }

        [DataMember]
        public DateTime CompletedOn { get; set; }

        [DataMember]
        public int Id { get; set; }
    }
}