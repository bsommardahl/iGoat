namespace iGoat.Domain
{
    public interface IUserRepository
    {
        User GetUser(string username, string password);
        void UpdateNewAuthKey(User user, string authKey);
        User GetUser(string authKey);
    }
}