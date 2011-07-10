using iGoat.Domain;
using iGoat.Domain.Entities;
using iGoat.Service;
using iGoat.Service.Contracts;
using Machine.Specifications;
using NHibernate;
using StructureMap;

namespace iGoat.Data.Specs.Integration
{
    public class when_loggin_in_a_user
    {
        private const string Password = "some password";
        private const string UserName = "some username";
        private const string AuthKey = "some auth key";
        private static IContainer _container;
        private static IDeliveryWebService _service;
        private static SuccessfulLoginResponse _result;
        private static ISession _session;
        private static Profile _profile;

        private Cleanup after = () =>
                                    {
                                        using (ITransaction tx = _session.BeginTransaction())
                                        {
                                            _session.Delete(_profile);
                                            tx.Commit();
                                        }
                                    };

        private Establish context = () =>
                                        {
                                            _container = new Container();
                                            new Bootstrapper(_container).Run();

                                            _session = _container.GetInstance<ISession>();

                                            _profile = new Profile
                                                           {
                                                               UserName = UserName,
                                                               Password = Password,
                                                               CurrentAuthKey = AuthKey,
                                                               Status = UserStatus.Active,
                                                           };

                                            using (ITransaction tx = _session.BeginTransaction())
                                            {
                                                _session.Save(_profile);
                                                tx.Commit();
                                            }

                                            _service = _container.GetInstance<IDeliveryWebService>();
                                        };

        private Because of = () => _result = _service.Login(UserName, Password);

        private It should_return_the_an_auth_key = () => _result.AuthKey.ShouldNotBeEmpty();
    }
}