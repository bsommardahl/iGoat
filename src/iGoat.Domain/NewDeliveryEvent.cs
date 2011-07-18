using System;
using iGoat.Domain.Entities;

namespace iGoat.Domain
{
    public class NewDeliveryEvent : IEvent
    {
        public Location Location { get; set; }

        public DateTime CompletedOn { get; set; }

        #region IEvent Members

        public string AuthKey { get; set; }

        #endregion
    }
}