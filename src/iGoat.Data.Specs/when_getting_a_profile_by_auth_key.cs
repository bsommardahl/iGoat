﻿using System;
using System.Collections.Generic;
using iGoat.Domain;
using iGoat.Domain.Entities;
using Machine.Specifications;
using NCommons.Testing.Equality;
using NHibernate;

namespace iGoat.Data.Specs
{
    public class when_getting_a_profile_by_auth_key : given_a_profile_repository
    {
        private const string InstanceKey = "some auth key";
        private static Profile _result;
        private static DeliveryItemType _itemType;
        private static DeliveryItem _expectedDeliveryItem;
        private static Profile _expectedProfile;
        private static Delivery _expectedDelivery;
        private static Location _location;
        private static Instance _currentInstance;
        private static DateTime _expires;

        private Establish context = () =>
                                        {
                                            DateTime now = new DateTime(2020, 1, 1);
                                            _expires = now.AddDays(1);
                                            MockTimeProvider.Setup(x => x.Now()).Returns(now);

                                            _itemType = new DeliveryItemType
                                                            {
                                                                Name = "something",
                                                            };

                                            _expectedDeliveryItem = new DeliveryItem
                                                                        {
                                                                            Status = DeliveryItemStatus.Delivered,
                                                                            ItemType = _itemType,
                                                                        };

                                            _location = new Location
                                                            {
                                                                Latitude = 86.6543m,
                                                                Longitue = -35.543m,
                                                                Name = "some name",
                                                            };

                                            _expectedDelivery = new Delivery
                                                                    {
                                                                        Items = new List<DeliveryItem>
                                                                                    {
                                                                                        _expectedDeliveryItem,
                                                                                    },
                                                                        CompletedOn = new DateTime(2010, 1, 1),
                                                                        Location = _location
                                                                    };

                                            _currentInstance = new Instance
                                                                   {
                                                                       AuthKey = InstanceKey,
                                                                       Expires = _expires,
                                                                   };

                                            _expectedProfile = new Profile
                                                                   {
                                                                       Status = UserStatus.Active,
                                                                       UserName = "some username",
                                                                       Password = "some password",
                                                                       CurrentInstance = _currentInstance,
                                                                       Items = new List<DeliveryItem>
                                                                                   {
                                                                                       _expectedDeliveryItem,
                                                                                   },
                                                                       Deliveries = new List<Delivery>
                                                                                        {
                                                                                            _expectedDelivery,
                                                                                        }
                                                                   };

                                            using (ITransaction tx = Session.BeginTransaction())
                                            {
                                                Session.Save(_currentInstance);
                                                Session.Save(_itemType);
                                                Session.Save(_expectedDeliveryItem);
                                                Session.Save(_location);
                                                Session.Save(_expectedDelivery);
                                                Session.Save(_expectedProfile);
                                                tx.Commit();
                                            }

                                            Session.Clear();
                                        };

        private Because of = () => _result = ProfileRepository.Get(InstanceKey);

        private It should_return_the_expected_profile = () =>
                                                            {
                                                                _expectedProfile.ToExpectedObject().ShouldEqual(_result);
                                                            };
    }
}