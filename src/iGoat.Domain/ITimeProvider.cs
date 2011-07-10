using System;

namespace iGoat.Domain
{
    public interface ITimeProvider
    {
        DateTime Now();
    }
}