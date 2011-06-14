using Machine.Specifications;

namespace iGoat.Domain.Specs
{
    public class when_getting_an_auth_key : given_a_security_service_context
    {
        private const string ExpectedAuthKey = "expected auth key";
        private const string Username = "some username";
        private const string Password = "some password";
        private static string _result;
        private static User _userFromRepository;

        private Establish a_context = () =>
                                          {
                                              _userFromRepository = new User
                                                                        {
                                                                            Status = UserStatus.Active,
                                                                        };

                                              MockUserRepository
                                                  .Setup(x => x.GetUser(Moq.It.Is<string>(y => y == Username),
                                                                        Moq.It.Is<string>(y => y == Password)))
                                                  .Returns(_userFromRepository);

                                              MockAuthKeyProvider
                                                  .Setup(x => x.GetNewAuthKey(Moq.It.Is<string>(y => y == Username),
                                                                              Moq.It.Is<string>(y => y == Password)))
                                                  .Returns(ExpectedAuthKey);
                                          };

        private Because of = () => _result = SecurityService.GetAuthKey(Username, Password);

        private It should_return_the_expected_auth_key =
            () => _result.ShouldEqual(ExpectedAuthKey);

        private It should_save_the_new_auth_key_to_the_user_object =
            () =>
            MockUserRepository.Verify(
                x =>
                x.UpdateNewAuthKey(Moq.It.Is<User>(y => y == _userFromRepository),
                                   Moq.It.Is<string>(y => y == ExpectedAuthKey)));
    }
}