namespace iGoat.Domain
{
    public interface ISecurityService
    {
        string GetAuthKey(string userName, string password);
        User GetUser(AuthorizationRequest authorizationRequest);
    }
}