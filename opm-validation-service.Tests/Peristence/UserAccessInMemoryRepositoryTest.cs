using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence
{
    public class UserAccessInMemoryRepositoryTest : UserAccessRepositoryTestBase
    {
        protected override IUserAccessRepository CreateRepository()
        {
            return new UserAccessInMemoryRepository();
        }
    }
}