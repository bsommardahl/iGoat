﻿using System;
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

        public ProfileRepository(ISession session)
        {
            _session = session;
        }

        public Profile Get(string username, string password)
        {
            using(_session.BeginTransaction())
            {
                var profile = _session.Linq<Profile>().FirstOrDefault(x => x.UserName == username && x.Password == password);

                if (profile == null)
                    throw new UnauthorizedAccessException("Invalid username or password.");

                return profile;
            }            
        }

        public void UpdateNewAuthKey(int profileId, string authKey)
        {
            //using(var transaction = _session.BeginTransaction())
            //{
                var profile = _session.Linq<Profile>().FirstOrDefault(x => x.Id == profileId);
                profile.CurrentAuthKey = authKey;
                _session.SaveOrUpdate(profile);
                //transaction.Commit();                
            //}
        }

        public Profile Get(string authKey)
        {
            using (_session.BeginTransaction())
            {
                var user = _session.Linq<Profile>().FirstOrDefault(x => x.CurrentAuthKey == authKey);

                if (user == null)
                    throw new UnauthorizedAccessException("Invalid auth key.");

                return user;
            }            
        }
    }
}