using System;
using System.Collections.Generic;
using System.Linq;
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
        private static Location roger;
        private static Location byron;
        private static Delivery d1;
        private static Delivery d2;

        private static void Main(string[] args)
        {
            var container = new Container();
            var bootstrapper = new Bootstrapper(container);
            bootstrapper.Run();

            FluentConfiguration config = bootstrapper.GetFluentConfiguration()
                .ExposeConfiguration(cfg =>
                                         {
                                             var schema = new SchemaExport(cfg);
                                             schema.Drop(false, true);
                                             schema.Create(false, true);
                                         });

            ISessionFactory sessionFactory = config.BuildSessionFactory();
            ISession session = sessionFactory.OpenSession();

            using (ITransaction transaction = session.BeginTransaction())
            {
                PersistDeliveryItemTypes(session);

                var listOfMyItems = PersistAndGetListOfDeliveryItems(session);

                PersistLocations(session);

                PersistDeliveries(session, listOfMyItems.Where(x => x.Status == DeliveryItemStatus.Delivered).ToList());

                session.Save(new Profile
                                 {
                                     UserName = "hondo",
                                     Password = "hondo1",
                                     Status = UserStatus.Active,
                                     Items = listOfMyItems,
                                     Deliveries = new List<Delivery>
                                                      {
                                                          d1, d2
                                                      },                                                      
                                 });

                transaction.Commit();
            }
        }

        private static void PersistDeliveries(ISession session, List<DeliveryItem> listOfDeliveredItems)
        {
            d1 = new Delivery
                     {
                         CompletedOn = new DateTime(2010, 5, 4),
                         Location = byron,
                         Items = listOfDeliveredItems.GetRange(0,3),
                     };
            session.Save(d1);

            d2 = new Delivery
                     {
                         CompletedOn = new DateTime(2010, 3, 2),
                         Location = roger,
                         Items = listOfDeliveredItems.GetRange(2, 2),
                     };
            session.Save(d2);
        }

        private static void PersistLocations(ISession session)
        {
            byron = new Location
                        {
                            Name = "Byron's House",
                            Latitude = 86.543m,
                            Longitue = -36.543m,
                        };
            session.Save(byron);

            roger = new Location
                        {
                            Name = "Roger's House",
                            Latitude = 56.543m,
                            Longitue = -26.543m,
                        };
            session.Save(roger);
        }

        private static List<DeliveryItem> PersistAndGetListOfDeliveryItems(ISession session)
        {
            var listOfItems = new List<DeliveryItem>
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


                                                new DeliveryItem
                                                   {ItemType = goat, Status = DeliveryItemStatus.Delivered},
                                               new DeliveryItem
                                                   {ItemType = chicken, Status = DeliveryItemStatus.Delivered},
                                               new DeliveryItem
                                                   {ItemType = chicken, Status = DeliveryItemStatus.Delivered},
                                               new DeliveryItem
                                                   {ItemType = dragon, Status = DeliveryItemStatus.Delivered},
                                               new DeliveryItem
                                                   {ItemType = goat, Status = DeliveryItemStatus.Delivered},


                                                new DeliveryItem
                                                   {ItemType = dragon, Status = DeliveryItemStatus.InTruck},
                                               new DeliveryItem
                                                   {ItemType = chicken, Status = DeliveryItemStatus.InTruck},
                                               new DeliveryItem
                                                   {ItemType = chicken, Status = DeliveryItemStatus.InTruck},
                                               new DeliveryItem
                                                   {ItemType = chicken, Status = DeliveryItemStatus.InTruck},
                                               new DeliveryItem
                                                   {ItemType = goat, Status = DeliveryItemStatus.InTruck},
                                          };
            listOfItems.ForEach(x => session.Save(x));

            return listOfItems;
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