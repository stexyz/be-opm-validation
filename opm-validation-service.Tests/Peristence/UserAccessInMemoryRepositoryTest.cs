using System;
using NUnit.Framework;
using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence
{
    [TestFixture]
    public class UserAccessInMemoryRepositoryTest : UserAccessRepositoryTestBase
    {
        protected override IUserAccessRepository CreateRepository()
        {
            return new UserAccessInMemoryRepository();
        }
    }
}