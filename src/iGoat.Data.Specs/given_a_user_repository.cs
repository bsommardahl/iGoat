using System.Reflection;
using iGoat.Domain;
using Machine.Specifications;
using Moq;

namespace iGoat.Data.Specs
{
    public abstract class given_a_user_repository : InMemoryDatabaseSpecificationFor<UserMap>
    {
        protected static IUserRepository UserRepository;

        protected Establish Context = () =>
                                          {
                                              UserRepository = new UserRepository(Session);
                                          };
    }
}