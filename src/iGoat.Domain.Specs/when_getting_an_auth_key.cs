using iGoat.Domain.Entities;
using Machine.Specifications;

namespace iGoat.Domain.Specs
{
    public class when_getting_an_auth_key : given_a_profile_service_context
    {
        private const string ExpectedAuthKey = "expected auth key";
        private const string Username = "some username";
        private const string Password = "some password";
        private static string _result;
        private static Profile _profileFromRepository;

        private Establish a_context = () =>
                                          {
                                              _profileFromRepository = new Profile
                                                                        {
                                                                            Status = UserStatus.Active,
                                                                        };

                                              MockUserRepository
                                                  .Setup(x => x.Get(Moq.It.Is<string>(y => y == Username),
                                                                        Moq.It.Is<string>(y => y == Password)))
                                                  .Returns(_profileFromRepository);

                                              MockAuthKeyProvider
                                                  .Setup(x => x.GetNewAuthKey(Moq.It.Is<string>(y => y == Username),
                                                                              Moq.It.Is<string>(y => y == Password)))
                                                  .Returns(ExpectedAuthKey);
                                          };

        private Because of = () => _result = ProfileService.GetAuthKey(Username, Password);

        private It should_return_the_expected_auth_key =
            () => _result.ShouldEqual(ExpectedAuthKey);

        private It should_save_the_new_auth_key_to_the_user_object =
            () =>
            MockUserRepository.Verify(
                x =>
                x.UpdateNewAuthKey(Moq.It.Is<Profile>(y => y == _profileFromRepository),
                                   Moq.It.Is<string>(y => y == ExpectedAuthKey)));
    }
}