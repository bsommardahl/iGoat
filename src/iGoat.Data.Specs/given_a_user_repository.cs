using iGoat.Domain;
using iGoat.Domain.Entities;
using Machine.Specifications;

namespace iGoat.Data.Specs
{
    public abstract class given_a_user_repository : InMemoryDatabaseSpecificationFor<Profile>
    {
        protected static IProfileRepository ProfileRepository;

        protected Establish Context = () => { ProfileRepository = new ProfileRepository(Session); };
    }
}