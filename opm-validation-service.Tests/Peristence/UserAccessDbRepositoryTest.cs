using NUnit.Framework;
using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence
{
    [TestFixture]
    public class UserAccessDbRepositoryTest : UserAccessRepositoryTestBase {
        protected override IUserAccessRepository CreateRepository() {
            DbRepositoryUtil.RecreateDatabase();
            DbRepositoryUtil.FillSampleOpm();
            return new UserAccessDbRepository();
        }
    }
}