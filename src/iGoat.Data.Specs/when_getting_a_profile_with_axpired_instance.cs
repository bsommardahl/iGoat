using System;
using System.Collections.Generic;
using iGoat.Domain;
using iGoat.Domain.Entities;
using Machine.Specifications;
using NHibernate;

namespace iGoat.Data.Specs
{
    public class when_getting_a_profile_with_expired_instance : given_a_user_repository
    {
        private const string Key = "some key";
        private static Exception _exception;
        private static Profile _expectedProfile;

        private Establish context = () =>
                                        {
                                            var now = new DateTime(2020, 1, 1);
                                            _expires = now.AddDays(-1);
                                            MockTimeProvider.Setup(x => x.Now()).Returns(now);

                                            var currentInstance = new Instance
                                                                      {
                                                                          AuthKey = Key,
                                                                          Expires = _expires,
                                                                      };

                                            _expectedProfile = new Profile
                                                                   {
                                                                       Status = UserStatus.Active,
                                                                       UserName = "some username",
                                                                       Password = "some password",
                                                                       CurrentInstance = currentInstance,
                                                                       Items = new List<DeliveryItem>(),
                                                                       Deliveries = new List<Delivery>(),
                                                                   };

                                            using (ITransaction tx = Session.BeginTransaction())
                                            {
                                                Session.Save(currentInstance);
                                                Session.Save(_expectedProfile);
                                                tx.Commit();
                                            }

                                            Session.Clear();
                                        };

        private Because of = () => _exception = Catch.Exception(() => ProfileRepository.Get(Key));

        private It should_throw_the_correct_type_of_exception =
            () => _exception.ShouldBeOfType<UnauthorizedAccessException>();

        private It should_throw_the_expected_exception_message =
            () =>
            _exception.Message.ShouldEqual(string.Format("Instance expired on {0:d} at {0:t}.", _expires));

        private static DateTime _expires;
    }
}