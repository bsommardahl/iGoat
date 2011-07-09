using System;
using iGoat.Domain.Entities;
using Machine.Specifications;
using It = Moq.It;

namespace iGoat.Domain.Specs
{
    public class when_getting_an_auth_key_for_inactive_user : given_a_profile_service_context
    {
        private const string Username = "some username";
        private const string Password = "some password";
        private static Exception _exception;

        private Establish context = () => MockUserRepository
                                              .Setup(x => x.Get(It.Is<string>(y => y == Username),
                                                                    It.Is<string>(y => y == Password)))
                                              .Returns(new Profile
                                                           {
                                                               Status = UserStatus.Inactive,
                                                           });

        private Because of = () => _exception = Catch.Exception(() => ProfileService.GetAuthKey(Username, Password));

        private Machine.Specifications.It should_throw_the_expected_exception = () => _exception.ShouldContainErrorMessage("Profile is inactive.");
    }
}