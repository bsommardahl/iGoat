using Machine.Specifications;

namespace iGoat.Domain.Specs
{
    public class when_providing_a_new_auth_key
    {
        private static GuidBasedAuthKeyProvider _provider;
        private static string _result;

        private Establish context = () => { _provider = new GuidBasedAuthKeyProvider(); };

        private Because of = () => _result = _provider.GetNewAuthKey("some username", "some password");

        private It should_return_an_auth_key = () => _result.ShouldNotBeNull();
    }
}