using System;

namespace iGoat.Domain
{
    public class SecurityService : ISecurityService
    {
        private readonly IAuthKeyProvider _authKeyProvider;
        private readonly IUserRepository _userRepository;

        public SecurityService(IAuthKeyProvider authKeyProvider, IUserRepository userRepository)
        {
            _authKeyProvider = authKeyProvider;
            _userRepository = userRepository;
        }

        #region ISecurityService Members

        public string GetAuthKey(string userName, string password)
        {
            User user = _userRepository.GetUser(userName, password);

            CheckStatus(user);

            string authKey = _authKeyProvider.GetNewAuthKey(userName, password);
            _userRepository.UpdateNewAuthKey(user, authKey);

            return authKey;
        }

        public User GetUser(AuthorizationRequest authorizationRequest)
        {
            User user = _userRepository.GetUser(authorizationRequest.AuthKey);
            CheckStatus(user);
            return user;
        }

        #endregion

        private static void CheckStatus(User user)
        {
            if (user.Status != UserStatus.Active)
                throw new UnauthorizedAccessException("User is inactive.");
        }
    }
}