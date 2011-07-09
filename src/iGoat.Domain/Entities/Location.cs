using System;

namespace iGoat.Domain.Entities
{
    public class Location
    {

        public virtual string Name { get; set; }

        public virtual decimal Latitude { get; set; }

        public virtual decimal Longitue { get; set; }

        public virtual int Id { get; set; }
    }
}