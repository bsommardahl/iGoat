using System;
using Machine.Specifications;
using NCommons.Testing.Equality;

namespace iGoat.Domain.Specs
{
    public class when_getting_a_user_that_doesnt_exist : given_a_security_service_context
    {
        private static Exception _exception;

        private Establish context = () => MockUserRepository.Setup(x => x.GetUser(AuthKey))
                                              .Returns(new User {Status = UserStatus.Inactive});

        private Because of =
            () => _exception = Catch.Exception(() => SecurityService.GetUser(new AuthorizationRequest{AuthKey = AuthKey}));

        private It should_throw_the_expected_exception = () => _exception.ShouldContainErrorMessage("User is inactive.");
        private const string AuthKey = "some authKey";
    }

    public class when_getting_a_user : given_a_security_service_context
    {
        private const string AuthKey = "some auth key";
        private static AuthorizationRequest _authRequest;
        private static User _result;
        private static User _expectedUser;

        private Establish context = () =>
                                        {
                                            _authRequest = new AuthorizationRequest
                                                               {
                                                                   AuthKey = AuthKey,
                                                               };

                                            _expectedUser = new User
                                                                {
                                                                    Status = UserStatus.Active
                                                                };

                                            MockUserRepository.Setup(
                                                x => x.GetUser(Moq.It.Is<string>(y => y == AuthKey)))
                                                .Returns(_expectedUser);
                                        };

        private Because of = () => _result = SecurityService.GetUser(_authRequest);

        private It should_return_the_expected_user = () => _expectedUser.ToExpectedObject().ShouldEqual(_result);
    }
}