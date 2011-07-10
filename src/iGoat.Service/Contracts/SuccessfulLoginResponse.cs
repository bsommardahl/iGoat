using System.Runtime.Serialization;

namespace iGoat.Service.Contracts
{
    [DataContract]
    public class SuccessfulLoginResponse
    {
        [DataMember]
        public string AuthKey { get; set; }
    }
}