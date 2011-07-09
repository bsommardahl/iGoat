using System.Collections.Generic;
using iGoat.Domain;
using iGoat.Domain.Entities;
using Machine.Specifications;
using NCommons.Testing.Equality;
using NHibernate;

namespace iGoat.Data.Specs
{
    public class when_getting_a_user_by_username_and_password : given_a_user_repository
    {
        private const string Username = "some username";
        private const string Password = "some password";
        private static Profile _result;
        private static Profile _expectedProfile;
        private static DeliveryItem _expectedDeliveryItem;
        private static DeliveryItemType _itemType;

        private Establish context = () =>
                                        {
                                            _itemType = new DeliveryItemType
                                                            {
                                                                Name = "something",
                                                            };

                                            _expectedDeliveryItem = new DeliveryItem
                                                                        {
                                                                            Status = DeliveryItemStatus.Assigned,
                                                                            ItemType = _itemType,
                                                                        };

                                            _expectedProfile = new Profile
                                                                   {
                                                                       Status = UserStatus.Active,
                                                                       UserName = Username,
                                                                       Password = Password,
                                                                       Items = new List<DeliveryItem>
                                                                                   {
                                                                                       _expectedDeliveryItem,
                                                                                   },
                                                                                   Deliveries = new List<Delivery>()
                                                                   };

                                            using (ITransaction tx = Session.BeginTransaction())
                                            {
                                                Session.Save(_itemType);
                                                Session.Save(_expectedDeliveryItem);
                                                Session.Save(_expectedProfile);
                                                tx.Commit();
                                            }

                                            Session.Clear();
                                        };

        private Because of = () => _result = ProfileRepository.Get(Username, Password);

        private It should_return_the_expected_user = () => _expectedProfile.ToExpectedObject().ShouldEqual(_result);
    }
}