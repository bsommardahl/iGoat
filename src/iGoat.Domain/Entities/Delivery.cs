using System;
using System.Collections.Generic;

namespace iGoat.Domain.Entities
{
    public class Delivery
    {
        public virtual IList<DeliveryItem> Items { get; set; }

        public virtual DateTime CompletedOn { get; set; }

        public virtual int Id { get; set; }

        public virtual Location Location { get; set; }
    }
}