using System;
using Machine.Specifications;

namespace iGoat.Data.Specs
{
    public class when_getting_a_profile_with_invalid_credentials : given_a_user_repository
    {
        private const string Username = "some username";
        private const string Password = "some password";
        private static Exception _exception;

        private Establish context = () => { };

        private Because of = () => _exception = Catch.Exception(() => ProfileRepository.GetUser(Username, Password));

        private It should_throw_the_expected_exception_message =
            () => _exception.Message.ShouldEqual("Invalid username or password.");

        private It should_throw_the_correct_type_of_exception =
            () => _exception.ShouldBeOfType<UnauthorizedAccessException>();
    }
}