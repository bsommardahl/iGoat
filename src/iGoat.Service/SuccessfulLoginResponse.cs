using System;
using System.Runtime.Serialization;

namespace iGoat.Service
{
    [DataContract]
    public class SuccessfulLoginResponse
    {
        [DataMember]
        public string AuthKey { get; set; }
    }
}