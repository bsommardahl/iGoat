using System;

namespace iGoat.Domain
{
    public class GuidBasedAuthKeyProvider : IAuthKeyProvider
    {
        public string GetNewAuthKey(string username, string password)
        {
            return Guid.NewGuid().ToString();
        }
    }
}