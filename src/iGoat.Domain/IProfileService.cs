using iGoat.Domain.Entities;

namespace iGoat.Domain
{
    public interface IProfileService
    {
        Instance GetAuthKey(string userName, string password);
        Profile GetProfile(string authKey);
    }
}
