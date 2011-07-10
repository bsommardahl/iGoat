using iGoat.Domain;
using iGoat.Service.Contracts;
using Machine.Specifications;
using It = Moq.It;

namespace iGoat.Service.Specs
{
    public class when_logging_a_user_in : given_a_delivery_service_context
    {
        private const string Username = "some username";
        private const string Password = "some password";
        private const string AuthKey = "some auth key";
        private static SuccessfulLoginResponse _result;

        private Establish context = () => MockProfileService
                                              .Setup(x => x.GetAuthKey(It.Is<string>(y => y == Username),
                                                                       It.Is<string>(y => y == Password)))
                                              .Returns(AuthKey);

        private Because of = () => _result = Service.Login(Username, Password);

        private Machine.Specifications.It should_return_the_expected_response_with_auth_key =
            () => _result.AuthKey.ShouldEqual(AuthKey);
    }
}