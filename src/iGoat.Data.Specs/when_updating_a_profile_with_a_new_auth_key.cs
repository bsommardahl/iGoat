﻿using System;
using System.Linq;
using iGoat.Domain;
using iGoat.Domain.Entities;
using Machine.Specifications;
using NHibernate;
using NHibernate.Linq;

namespace iGoat.Data.Specs
{
    public class when_updating_a_profile_with_a_new_auth_key : given_a_profile_repository
    {
        private const string AuthKey = "some new auth key";
        private static Profile _profile;
        private static DateTime _expireDate;

        private Establish context = () =>
                                        {
                                            _expireDate = new DateTime(2010, 3, 4);

                                            _profile = new Profile
                                                           {
                                                               UserName = "some username",
                                                               Password = "some password",
                                                               Status = UserStatus.Active,
                                                           };

                                            using (ITransaction tx = Session.BeginTransaction())
                                            {
                                                Session.Save(_profile);
                                                tx.Commit();
                                            }

                                            Session.Clear();
                                        };

        private Because of = () => ProfileRepository.UpdateNewAuthKey(_profile.Id, AuthKey, _expireDate);

        private It should_set_the_auth_key_on_the_user =
            () => Session.Linq<Profile>().Single(x => x.Id == _profile.Id).CurrentInstance.AuthKey.ShouldEqual(AuthKey);
    }
}