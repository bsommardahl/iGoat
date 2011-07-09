using iGoat.Domain.Entities;

namespace iGoat.Domain
{
    public interface IProfileRepository
    {
        Profile GetUser(string username, string password);
        void UpdateNewAuthKey(Profile profile, string authKey);
        Profile GetUser(string authKey);
    }
}