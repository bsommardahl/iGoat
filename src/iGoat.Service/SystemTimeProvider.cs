using System;
using iGoat.Domain;

namespace iGoat.Service
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}