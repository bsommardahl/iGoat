using System;
using Machine.Specifications;

namespace iGoat.Data.Specs
{
    public class when_getting_a_profile_with_invalid_auth_key : given_a_user_repository
    {
        private static Exception _exception;

        private Establish context = () => { };

        private Because of = () => _exception = Catch.Exception(() => ProfileRepository.GetUser("some auth key"));

        private It should_throw_the_expected_exception_message =
            () => _exception.Message.ShouldEqual("Invalid auth key.");

        private It should_throw_the_correct_type_of_exception =
            () => _exception.ShouldBeOfType<UnauthorizedAccessException>();
    }
}