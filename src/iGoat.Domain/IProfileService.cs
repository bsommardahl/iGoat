using iGoat.Domain.Entities;

namespace iGoat.Domain
{
    public interface IProfileService
    {
        string GetAuthKey(string userName, string password);
        Profile GetProfile(string authKey);
    }
}
