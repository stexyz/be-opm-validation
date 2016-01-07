using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence
{
    public class UserAccessDbRepositoryTest : UserAccessRepositoryTestBase {
        protected override IUserAccessRepository CreateRepository() {
            DbRepositoryUtil.RecreateDatabase();
            DbRepositoryUtil.FillSampleOpm();
            return new UserAccessDbRepository();
        }
    }
}