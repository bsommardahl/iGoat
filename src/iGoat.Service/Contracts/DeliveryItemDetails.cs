using System.Runtime.Serialization;

namespace iGoat.Service.Contracts
{
    [DataContract]
    public class DeliveryItemDetails
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Id { get; set; }
    }
}