using Machine.Specifications;
using Moq;

namespace iGoat.Domain.Specs
{
    public abstract class given_a_security_service_context
    {
        protected static ISecurityService SecurityService;
        protected static Mock<IAuthKeyProvider> MockAuthKeyProvider;
        protected static Mock<IUserRepository> MockUserRepository;

        protected Establish Context = () =>
                                          {
                                              MockUserRepository = new Mock<IUserRepository>();
                                              MockAuthKeyProvider = new Mock<IAuthKeyProvider>();
                                              SecurityService = new SecurityService(MockAuthKeyProvider.Object,
                                                                                    MockUserRepository.Object);
                                          };
    }
}