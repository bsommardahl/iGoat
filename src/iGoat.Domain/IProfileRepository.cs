using System;
using iGoat.Domain.Entities;

namespace iGoat.Domain
{
    public interface IProfileRepository
    {
        Profile Get(string username, string password);
        void UpdateNewAuthKey(int profileId, string key, DateTime expires);
        Profile Get(string key);
    }
}