using System;
using NUnit.Framework;
using opm_validation_service.Models;
using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence {
    [TestFixture]
    public class UserAccessInMemoryRepositoryTest
    {
        private readonly IUserAccessRepository _repository = new UserAccessInMemoryRepository();

        [Test]
        public void GetAndRecordTest()
        {
            const string userId = "userXYZ";
            IUser user = new User(userId);
            Assert.AreEqual(0, _repository.GetUserAccessCount(user, new TimeSpan(999,0,0)));
            _repository.RecordAccess(user, new EanEicCode(""));
            Assert.AreEqual(1, _repository.GetUserAccessCount(user, new TimeSpan(999, 0, 0)));
        }
    }
}
