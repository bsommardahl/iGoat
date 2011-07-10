using System;
using iGoat.Domain.Entities;
using iGoat.Service.Contracts;
using Machine.Specifications;
using NCommons.Testing.Equality;

namespace iGoat.Service.Specs
{
    public class when_logging_a_user_in : given_a_delivery_service_context
    {
        private const string Username = "some username";
        private const string Password = "some password";
        private const string Key = "some auth key";
        private static SuccessfulLoginResponse _result;
        private static readonly DateTime ExpireDate = new DateTime(2010, 1, 1);
        private static SuccessfulLoginResponse _expectedResponse;
        private static Instance _instance;

        private Establish context = () =>
                                        {
                                            _instance = new Instance
                                                           {
                                                               Id = 1,
                                                               AuthKey = Key,
                                                               Expires = ExpireDate,
                                                           };

                                            MockProfileService
                                                .Setup(x => x.GetAuthKey(Moq.It.Is<string>(y => y == Username),
                                                                         Moq.It.Is<string>(y => y == Password)))
                                                .Returns(_instance);

                                            _expectedResponse = new SuccessfulLoginResponse
                                                                    {
                                                                        AuthKey = Key,
                                                                        Expires = ExpireDate,
                                                                    };
                                        };

        private Because of = () => _result = Service.Login(Username, Password);

        private It should_return_the_expected_response_with_auth_key =
            () => _result.AuthKey.ToExpectedObject().ShouldEqual(_result.AuthKey);
    }
}