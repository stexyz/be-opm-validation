using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence
{
    public class OpmInMemoryRepositoryTest : OpmRepositoryTestBase
    {
        protected override IOpmRepository GetOpmRepository()
        {
            return new OpmInMemoryRepository();
        }
    }
}