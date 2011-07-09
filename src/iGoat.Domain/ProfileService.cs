using System;
using iGoat.Domain.Entities;

namespace iGoat.Domain
{
    public class ProfileService : IProfileService
    {
        private readonly IAuthKeyProvider _authKeyProvider;
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IAuthKeyProvider authKeyProvider, IProfileRepository profileRepository)
        {
            _authKeyProvider = authKeyProvider;
            _profileRepository = profileRepository;
        }

        #region ISecurityService Members

        public string GetAuthKey(string userName, string password)
        {
            Profile profile = _profileRepository.GetUser(userName, password);

            CheckStatus(profile);

            string authKey = _authKeyProvider.GetNewAuthKey(userName, password);
            _profileRepository.UpdateNewAuthKey(profile, authKey);

            return authKey;
        }

        public Profile GetProfile(string authKey)
        {
            Profile profile = _profileRepository.GetUser(authKey);
            CheckStatus(profile);
            return profile;
        }

        #endregion

        private static void CheckStatus(Profile profile)
        {
            if (profile.Status != UserStatus.Active)
                throw new UnauthorizedAccessException("Profile is inactive.");
        }
    }
}