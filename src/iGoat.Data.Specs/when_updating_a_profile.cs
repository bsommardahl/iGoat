using System;
using System.Collections.Generic;
using System.Linq;
using iGoat.Domain;
using iGoat.Domain.Entities;
using Machine.Specifications;
using NCommons.Testing.Equality;
using NHibernate;
using NHibernate.Linq;

namespace iGoat.Data.Specs
{
    public class when_updating_a_profile : given_a_profile_repository
    {
        private static Profile _profileToUpdate;
        private static Profile _result;
        private static Profile _originalProfileInDatabase;

        private Establish context = () =>
                                        {
                                            _originalProfileInDatabase = new Profile
                                                                             {
                                                                                 UserName = "some username",
                                                                                 Password = "some password",
                                                                                 Status = UserStatus.Active,
                                                                             };

                                            using (ITransaction tx = Session.BeginTransaction())
                                            {
                                                Session.Save(_originalProfileInDatabase);
                                                tx.Commit();
                                            }

                                            Session.Clear();

                                            _deliveryItem = new DeliveryItem
                                                                {
                                                                    ItemType = new DeliveryItemType
                                                                                   {
                                                                                       Name = "Goat",
                                                                                   },
                                                                    Status = DeliveryItemStatus.InTruck,
                                                                };

                                            _profileToUpdate = new Profile
                                                                   {
                                                                       Id = _originalProfileInDatabase.Id,
                                                                       UserName = _originalProfileInDatabase.UserName,
                                                                       Password = _originalProfileInDatabase.Password,
                                                                       Status = _originalProfileInDatabase.Status,
                                                                       Deliveries = new List<Delivery>
                                                                                        {
                                                                                            new Delivery
                                                                                                {
                                                                                                    CompletedOn =
                                                                                                        new DateTime(
                                                                                                        2010, 1, 1),
                                                                                                    Location =
                                                                                                        new Location
                                                                                                            {
                                                                                                                Latitude
                                                                                                                    = 1,
                                                                                                                Longitue
                                                                                                                    = 2,
                                                                                                            },
                                                                                                    Items =
                                                                                                        new List
                                                                                                        <DeliveryItem>
                                                                                                            {
                                                                                                                _deliveryItem,
                                                                                                            }
                                                                                                }
                                                                                        },
                                                                       Items = new List<DeliveryItem>
                                                                                   {
                                                                                       _deliveryItem,
                                                                                   }
                                                                   };
                                        };

        private Because of = () => _result = ProfileRepository.Update(_profileToUpdate);

        private It should_return_an_updated_profile_complete_with_keys =
            () => _profileToUpdate.ToExpectedObject().ShouldEqual(_result);

        private It should_save_the_changes_to_the_database = () =>
                                                                 {
                                                                     var profileFromDatabase =
                                                                         Session.Linq<Profile>().Single(
                                                                             x => x.Id == _originalProfileInDatabase.Id);
                                                                     _profileToUpdate.ToExpectedObject().ShouldEqual(
                                                                         profileFromDatabase);
                                                                 };
        private static DeliveryItem _deliveryItem;
    }
}