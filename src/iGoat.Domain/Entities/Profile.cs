using System;
using System.Collections.Generic;

namespace iGoat.Domain.Entities
{
    public class Profile
    {
        public virtual UserStatus Status { get; set; }

        public virtual int Id { get; set; }

        public virtual string Password { get; set; }

        public virtual string UserName { get; set; }

        public virtual string CurrentAuthKey { get; set; }

        public virtual IList<DeliveryItem> Items { get; set; }

        public virtual IList<Delivery> Deliveries { get; set; }
    }
}