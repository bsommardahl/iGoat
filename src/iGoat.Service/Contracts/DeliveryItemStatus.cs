using System.Runtime.Serialization;

namespace iGoat.Service.Contracts
{
    [DataContract]        
    public enum DeliveryItemStatus
    {
        [EnumMember]
        Assigned = 1,
        [EnumMember]
        InTruck = 2,
        [EnumMember]
        Delivered = 3,
    }    
}