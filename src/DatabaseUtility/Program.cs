using FluentNHibernate.Cfg;
using iGoat.Data;
using iGoat.Domain;
using iGoat.Domain.Entities;
using iGoat.Service;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using StructureMap;

namespace DatabaseUtility
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = new Container();
            var bootstrapper = new Bootstrapper(container);
            bootstrapper.Run();

            FluentConfiguration config = bootstrapper.GetFluentConfiguration()
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true));

            ISessionFactory sessionFactory = config.BuildSessionFactory();
            ISession session = sessionFactory.OpenSession();

            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(new Profile
                                 {
                                     UserName = "hondo",
                                     Password = "hondo1",
                                     Status = UserStatus.Active,
                                 });

                transaction.Commit();
            }
        }
    }
}