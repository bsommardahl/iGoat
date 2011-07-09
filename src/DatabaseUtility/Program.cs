using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg;
using iGoat.Data;
using iGoat.Domain;
using iGoat.Domain.Entities;
using iGoat.Service;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using StructureMap;
using DeliveryItemStatus = iGoat.Domain.DeliveryItemStatus;

namespace DatabaseUtility
{
    internal class Program
    {
        private static DeliveryItemType kanga;
        private static DeliveryItemType goat;
        private static DeliveryItemType chicken;
        private static DeliveryItemType dragon;

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
                PersistDeliveryItemTypes(session);

                var listOfAssignedItems = PersistAndGetListOfAssignedItems(session);

                List<DeliveryItem> listOfDeliveredItems = PersistAndGetListOfDeliveredItems(session);

                var byron = new Location
                                   {
                                       Name = "Byron's House",
                                       Latitude = 86.543m,
                                       Longitue = -36.543m,
                                   };
                session.Save(byron);

                var d1 = new Delivery
                             {
                                 CompletedOn = new DateTime(2010, 5, 4),
                                 Location = byron,
                                 Items = listOfDeliveredItems,
                             };
                session.Save(d1);

                session.Save(new Profile
                                 {
                                     UserName = "hondo",
                                     Password = "hondo1",
                                     Status = UserStatus.Active,
                                     Items = listOfAssignedItems,
                                     Deliveries = new List<Delivery>
                                                      {
                                                          d1,
                                                      },                                                      
                                 });

                transaction.Commit();
            }
        }

        private static List<DeliveryItem> PersistAndGetListOfAssignedItems(ISession session)
        {
            var listOfAssignedItems = new List<DeliveryItem>
                                          {
                                              new DeliveryItem
                                                  {ItemType = goat, Status = DeliveryItemStatus.Assigned},
                                              new DeliveryItem
                                                  {ItemType = chicken, Status = DeliveryItemStatus.Assigned},
                                              new DeliveryItem
                                                  {ItemType = dragon, Status = DeliveryItemStatus.Assigned},
                                              new DeliveryItem
                                                  {ItemType = goat, Status = DeliveryItemStatus.Assigned},
                                              new DeliveryItem
                                                  {ItemType = kanga, Status = DeliveryItemStatus.Assigned},
                                          };
            listOfAssignedItems.ForEach(x => session.Save(x));

            return listOfAssignedItems;
        }

        private static List<DeliveryItem> PersistAndGetListOfDeliveredItems(ISession session)
        {
            var listOfDeliveredItems = new List<DeliveryItem>
                                           {
                                               new DeliveryItem
                                                   {ItemType = goat, Status = DeliveryItemStatus.Delivered},
                                               new DeliveryItem
                                                   {ItemType = chicken, Status = DeliveryItemStatus.Delivered},
                                               new DeliveryItem
                                                   {ItemType = dragon, Status = DeliveryItemStatus.Delivered},
                                               new DeliveryItem
                                                   {ItemType = goat, Status = DeliveryItemStatus.Delivered},
                                           };
            listOfDeliveredItems.ForEach(x => session.Save(x));
            return listOfDeliveredItems;
        }

        private static void PersistDeliveryItemTypes(ISession session)
        {
            goat = new DeliveryItemType
                       {
                           Name = "Goat",
                       };
            session.Save(goat);

            chicken = new DeliveryItemType
                          {
                              Name = "Chicken",
                          };
            session.Save(chicken);

            dragon = new DeliveryItemType
                         {
                             Name = "Dragon",
                         };
            session.Save(dragon);

            kanga = new DeliveryItemType
                        {
                            Name = "Kangaroo",
                        };
            session.Save(kanga);
        }
    }
}