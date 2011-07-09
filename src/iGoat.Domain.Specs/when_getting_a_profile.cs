using FizzWare.NBuilder;
using iGoat.Domain.Entities;
using Machine.Specifications;
using NCommons.Testing.Equality;

namespace iGoat.Domain.Specs
{
    public class when_getting_a_profile : given_a_profile_service_context
    {
        private const string AuthKey = "some auth key";
        private static Profile _result;
        private static Profile _expectedProfile;

        private Establish context = () =>
                                        {
                                            _expectedProfile = new Profile
                                                                   {
                                                                       Status = UserStatus.Active,
                                                                       Items =
                                                                           Builder<DeliveryItem>.CreateListOfSize(3).
                                                                           Build(),
                                                                       Deliveries =
                                                                           Builder<Delivery>.CreateListOfSize(5).Build()
                                                                   };

                                            MockUserRepository.Setup(
                                                x => x.GetUser(Moq.It.Is<string>(y => y == AuthKey)))
                                                .Returns(_expectedProfile);
                                        };

        private Because of = () => _result = ProfileService.GetProfile(AuthKey);

        private It should_return_the_expected_user = () => _expectedProfile.ToExpectedObject().ShouldEqual(_result);
    }
}