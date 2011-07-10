using System.Runtime.Serialization;

namespace iGoat.Service.Contracts
{
    [DataContract]
    public class DeliveryItemSummary
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}