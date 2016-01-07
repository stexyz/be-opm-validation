using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence
{
    public class OpmDbRepositoryTest : OpmRepositoryTestBase
    {
        protected override IOpmRepository GetOpmRepository()
        {
            DbRepositoryUtil.RecreateDatabase();
            DbRepositoryUtil.FillSampleOpm("OpmRepoSampleData.csv");
            return new OpmDbRepository();
        }
    }
}