using System;
using iGoat.Domain.Entities;

namespace iGoat.Domain
{
    public class ProfileService : IProfileService
    {
        private readonly IAuthKeyProvider _authKeyProvider;
        private readonly IProfileRepository _profileRepository;
        private readonly ITimeProvider _timeProvider;

        public ProfileService(IAuthKeyProvider authKeyProvider, IProfileRepository profileRepository, ITimeProvider timeProvider)
        {
            _authKeyProvider = authKeyProvider;
            _profileRepository = profileRepository;
            _timeProvider = timeProvider;
        }

        #region ISecurityService Members

        public Instance GetAuthKey(string userName, string password)
        {
            Profile profile = _profileRepository.Get(userName, password);

            CheckStatus(profile);

            string authKey = _authKeyProvider.GetNewAuthKey(userName, password);
            var expires = _timeProvider.Now().AddHours(1);
            _profileRepository.UpdateNewAuthKey(profile.Id, authKey, expires);

            return new Instance()
                       {
                           AuthKey = authKey,
                           Expires = expires,
                       };
        }

        public Profile GetProfile(string authKey)
        {
            Profile profile = _profileRepository.Get(authKey);
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