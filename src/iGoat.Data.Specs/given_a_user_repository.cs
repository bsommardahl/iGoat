using iGoat.Domain;
using iGoat.Domain.Entities;
using Machine.Specifications;
using Moq;

namespace iGoat.Data.Specs
{
    public abstract class given_a_user_repository : InMemoryDatabaseSpecificationFor<Profile>
    {
        protected static IProfileRepository ProfileRepository;
        protected static Mock<ITimeProvider> MockTimeProvider;

        protected Establish Context = () =>
                                          {
                                              MockTimeProvider = new Mock<ITimeProvider>();
                                              ProfileRepository = new ProfileRepository(Session, MockTimeProvider.Object);
                                          };
    }
}