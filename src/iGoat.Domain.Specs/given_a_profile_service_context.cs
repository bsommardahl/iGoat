using Machine.Specifications;
using Moq;

namespace iGoat.Domain.Specs
{
    public abstract class given_a_profile_service_context
    {
        protected static IProfileService ProfileService;
        protected static Mock<IAuthKeyProvider> MockAuthKeyProvider;
        protected static Mock<IProfileRepository> MockUserRepository;

        protected Establish Context = () =>
                                          {
                                              MockUserRepository = new Mock<IProfileRepository>();
                                              MockAuthKeyProvider = new Mock<IAuthKeyProvider>();
                                              ProfileService = new ProfileService(MockAuthKeyProvider.Object,
                                                                                    MockUserRepository.Object);
                                          };
    }
}