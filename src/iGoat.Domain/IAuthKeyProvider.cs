namespace iGoat.Domain
{
    public interface IAuthKeyProvider
    {
        string GetNewAuthKey(string username, string password);
    }
}