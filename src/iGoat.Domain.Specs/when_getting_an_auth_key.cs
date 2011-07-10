using System;
using iGoat.Domain.Entities;
using Machine.Specifications;
using NCommons.Testing.Equality;

namespace iGoat.Domain.Specs
{
    public class when_getting_a_new_instance : given_a_profile_service_context
    {
        private const string Key = "expected auth key";
        private const string Username = "some username";
        private const string Password = "some password";
        private static Instance _result;
        private static Profile _profileFromRepository;
        private static Instance _expectedInstance;
        private static readonly DateTime Now = new DateTime(2010, 4, 5);
        private static DateTime _expireDate;
        
        private Establish a_context = () =>
                                          {
                                              _expireDate = Now.AddHours(1);
                                              MockTimeProvider.Setup(x => x.Now()).Returns(Now);

                                              _expectedInstance = new Instance
                                                                      {
                                                                          AuthKey = Key,
                                                                          Expires = _expireDate,
                                                                      };

                                              _profileFromRepository = new Profile
                                                                           {
                                                                               Status = UserStatus.Active,
                                                                           };

                                              MockUserRepository
                                                  .Setup(x => x.Get(Moq.It.Is<string>(y => y == Username),
                                                                    Moq.It.Is<string>(y => y == Password)))
                                                  .Returns(_profileFromRepository);

                                              MockAuthKeyProvider
                                                  .Setup(x => x.GetNewAuthKey(Moq.It.Is<string>(y => y == Username),
                                                                              Moq.It.Is<string>(y => y == Password)))
                                                  .Returns(Key);                                              
                                          };

        private Because of = () => _result = ProfileService.GetAuthKey(Username, Password);

        private It should_return_the_expected_auth_key =
            () => _expectedInstance.ToExpectedObject().ShouldEqual(_result);

        private It should_save_the_new_auth_key_to_the_user_object =
            () =>
            MockUserRepository.Verify(
                x =>
                x.UpdateNewAuthKey(Moq.It.Is<int>(y => y == _profileFromRepository.Id),
                                   Moq.It.Is<string>(y => y == Key),
                                   Moq.It.Is<DateTime>(y => y == _expireDate)));
    }
}