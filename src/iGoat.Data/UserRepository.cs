using System;
using System.Linq;
using iGoat.Domain;
using NHibernate;
using NHibernate.Linq;

namespace iGoat.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ISession _session;

        public UserRepository(ISession session)
        {
            _session = session;
        }

        public User GetUser(string username, string password)
        {
            var user = _session.Linq<User>().FirstOrDefault(x => x.UserName == username && x.Password == password);

            return user;
        }

        public void UpdateNewAuthKey(User user, string authKey)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string authKey)
        {
            throw new NotImplementedException();
        }
    }
}