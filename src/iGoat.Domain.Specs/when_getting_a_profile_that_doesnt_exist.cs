using System;
using iGoat.Domain.Entities;
using Machine.Specifications;

namespace iGoat.Domain.Specs
{
    public class when_getting_a_profile_that_doesnt_exist : given_a_profile_service_context
    {
        private const string AuthKey = "some authKey";
        private static Exception _exception;

        private Establish context = () => MockUserRepository.Setup(x => x.GetUser(AuthKey))
                                              .Returns(new Profile {Status = UserStatus.Inactive});

        private Because of =
            () =>
            _exception = Catch.Exception(() => ProfileService.GetProfile(AuthKey));

        private It should_throw_the_expected_exception = () => _exception.ShouldContainErrorMessage("Profile is inactive.");
    }
}