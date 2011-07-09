using System;
using System.ServiceModel;
using Machine.Specifications;
using It = Moq.It;

namespace iGoat.Service.Specs
{
    public class when_logging_a_user_in_with_invalid_credentials : given_a_delivery_service_context
    {
        private const string Username = "some username";
        private const string Password = "some password";
        private const string ErrorMessage = "some error message";
        private static Exception _exception;

        private Establish context = () => MockProfileService
                                              .Setup(x => x.GetAuthKey(It.Is<string>(y => y == Username),
                                                                       It.Is<string>(y => y == Password)))
                                              .Throws(new UnauthorizedAccessException(ErrorMessage));

        private Because of = () => _exception = Catch.Exception(() => Service.Login(Username, Password));

        private Machine.Specifications.It should_throw_the_expected_fault_exception =
            () => _exception.ShouldBeOfType<FaultException<UnauthorizedAccessException>>();
    }
}