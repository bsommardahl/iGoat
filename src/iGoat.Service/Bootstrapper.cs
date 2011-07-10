using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using iGoat.Data;
using iGoat.Domain;
using iGoat.Domain.Entities;
using StructureMap;
using ISession = NHibernate.ISession;

namespace iGoat.Service
{
    public class Bootstrapper
    {
        private readonly IContainer _container;
        
        public Bootstrapper(IContainer container)
        {
            _container = container;
        }

        public void Run()
        {
            ConfigureContainer();
        }

        public FluentConfiguration GetFluentConfiguration()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                              .ConnectionString(
                                  c => c.FromConnectionStringWithKey("iGoatDatabase")).ShowSql())

                .Mappings(
                    x =>
                    x.AutoMappings.Add(
                        AutoMap.Assemblies(typeof (Profile).Assembly).Where(
                            assembly => assembly.Namespace.Contains("Entities"))));
        }

        public void ConfigureContainer()
        {
            _container.Configure(
                x =>
                    {
                        x.For<IDeliveryWebService>().Use<DeliveryWebService>();
                        x.For<ISession>().Singleton().Use(GetFluentConfiguration().BuildSessionFactory().OpenSession());
                        x.For<IProfileService>().Use<ProfileService>();
                        x.For<IProfileRepository>().Use<ProfileRepository>();
                        x.For<IAuthKeyProvider>().Use<GuidBasedAuthKeyProvider>();
                    });
        }
    }
}