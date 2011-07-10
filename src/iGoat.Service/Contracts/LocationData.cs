using System.Runtime.Serialization;

namespace iGoat.Service.Contracts
{
    [DataContract]
    public class LocationData
    {
        [DataMember]
        public decimal Longitude { get; set; }

        [DataMember]
        public decimal Latitude { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}