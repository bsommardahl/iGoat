using System;
using System.Linq;
using iGoat.Domain;
using iGoat.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace iGoat.Data
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ISession _session;
        private readonly ITimeProvider _timeProvider;

        public ProfileRepository(ISession session, ITimeProvider timeProvider)
        {
            _session = session;
            _timeProvider = timeProvider;
        }

        #region IProfileRepository Members

        public Profile Get(string username, string password)
        {
            using (_session.BeginTransaction())
            {
                Profile profile =
                    _session.Linq<Profile>().FirstOrDefault(x => x.UserName == username && x.Password == password);

                if (profile == null)
                    throw new UnauthorizedAccessException("Invalid username or password.");

                return profile;
            }
        }

        public void UpdateNewAuthKey(int profileId, string key, DateTime expires)
        {
            using (ITransaction transaction = _session.BeginTransaction())
            {
                Profile profile = _session.Linq<Profile>().FirstOrDefault(x => x.Id == profileId);
                
                var authKey = new Instance
                                  {
                                      AuthKey = key,
                                      Expires = expires,
                                  };
                _session.Save(authKey);

                profile.CurrentInstance = authKey;
                _session.SaveOrUpdate(profile);
                transaction.Commit();
            }
        }

        public Profile Get(string key)
        {
            using (_session.BeginTransaction())
            {
                var now = _timeProvider.Now();
                var profile =
                    _session.Linq<Profile>().SingleOrDefault(x => x.CurrentInstance.AuthKey == key);

                if (profile == null)
                    throw new UnauthorizedAccessException("Invalid auth key.");

                if(profile.CurrentInstance.Expires<now)
                    throw new UnauthorizedAccessException(string.Format("Instance expired on {0:d} at {0:t}.", profile.CurrentInstance.Expires));

                return profile;
            }
        }

        public Profile Update(Profile profile)
        {
            _session.SaveOrUpdate(profile);
            return profile;
        }

        #endregion
    }
}