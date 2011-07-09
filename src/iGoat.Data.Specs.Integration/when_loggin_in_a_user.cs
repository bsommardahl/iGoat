using System;
using System.Collections.Generic;
using iGoat.Domain;
using iGoat.Domain.Entities;
using iGoat.Service;
using Machine.Specifications;
using NHibernate;
using StructureMap;
using DeliveryItemStatus = iGoat.Domain.DeliveryItemStatus;

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

        private Establish context = () =>
                                        {
                                            _container = new Container();
                                            new Bootstrapper(_container).Run();

                                            _session = _container.GetInstance<ISession>();
                                            _session.Transaction.Begin();

                                            _itemType = new DeliveryItemType
                                                            {
                                                                Name = "something",
                                                            };

                                            _expectedDeliveryItem = new DeliveryItem
                                                                        {
                                                                            Status = DeliveryItemStatus.Assigned,
                                                                            ItemType = _itemType,
                                                                        };
                                            
                                            _session.Save(_itemType);
                                            _session.Save(_expectedDeliveryItem);

                                            _session.Save(new Profile
                                                              {
                                                                  Password = Password,
                                                                  UserName = UserName,
                                                                  CurrentAuthKey = AuthKey,
                                                                  Status = UserStatus.Active,
                                                                  Items = new List<DeliveryItem>
                                                                              {
                                                                                  _expectedDeliveryItem
                                                                              }
                                                              });
                                            
                                            _service = _container.GetInstance<IDeliveryWebService>();
                                        };

        private Because of = () => _result = _service.Login(UserName, Password);

        private It should_return_the_an_auth_key = () => _result.AuthKey.ShouldNotBeEmpty();

        private Cleanup after_finished = () => _session.Transaction.Rollback();
        private static DeliveryItem _expectedDeliveryItem;
        private static DeliveryItemType _itemType;
    }
}