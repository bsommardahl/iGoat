using System.Linq;
using iGoat.Domain;
using Machine.Specifications;
using Moq;
using NCommons.Testing.Equality;
using NHibernate.Linq;
using It = Machine.Specifications.It;

namespace iGoat.Data.Specs
{
    public class when_getting_a_user_by_username_and_password : given_a_user_repository
    {
        private const string Username = "some username";
        private const string Password = "some password";
        private static User _result;
        private static User _expectedUser;

        private Establish context = () =>
                                        {
                                            _expectedUser = new User
                                            {
                                                Id = 1,
                                                Status = UserStatus.Active,
                                            };

                                            using (var tx = Session.BeginTransaction())
                                            {
                                                Session.Save(_expectedUser);
                                                tx.Commit();
                                            }

                                            Session.Clear();                                            
                                        };

        private Because of = () => _result = UserRepository.GetUser(Username, Password);

        private It should_return_the_expected_user = () => _expectedUser.ToExpectedObject().ShouldEqual(_result);
    }
}