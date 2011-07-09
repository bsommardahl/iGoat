using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;

namespace iGoat.Data.Specs
{
    public abstract class InMemoryDatabaseSpecificationFor<TEntity> : IDisposable
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        protected static ISession Session;

        protected InMemoryDatabaseSpecificationFor()
        {
            if (_configuration == null)
            {
                _configuration = new Configuration()
                    .SetProperty(Environment.ReleaseConnections, "on_close")
                    .SetProperty(Environment.Dialect, typeof (SQLiteDialect).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionDriver, typeof (SQLite20Driver).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionString, "data source=:memory:")
                    .SetProperty(Environment.ProxyFactoryFactoryClass,
                                 typeof (ProxyFactoryFactory).AssemblyQualifiedName);

                var fluent = Fluently.Configure(_configuration)
                    .Mappings(
                        x =>
                        x.AutoMappings.Add(
                            AutoMap.Assemblies(typeof(TEntity).Assembly).Where(assembly => assembly.Namespace.Contains("Entities"))));

                _sessionFactory = fluent.BuildSessionFactory();
            }

            Session = _sessionFactory.OpenSession();

            new SchemaExport(_configuration).Execute(true, true, false, Session.Connection, Console.Out);
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}