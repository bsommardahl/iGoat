using System;

namespace iGoat.Domain.Entities
{
    public class Instance
    {
        public virtual string AuthKey { get; set; }

        public virtual DateTime Expires { get; set; }

        public virtual int Id { get; set; }        
    }
}