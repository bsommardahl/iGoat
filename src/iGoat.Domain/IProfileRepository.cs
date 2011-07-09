using iGoat.Domain.Entities;

namespace iGoat.Domain
{
    public interface IProfileRepository
    {
        Profile Get(string username, string password);
        void UpdateNewAuthKey(Profile profile, string authKey);
        Profile Get(string authKey);
    }
}