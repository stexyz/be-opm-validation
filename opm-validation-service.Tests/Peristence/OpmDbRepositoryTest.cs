using NUnit.Framework;
using opm_validation_service.Models;
using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence {
    [TestFixture]
    public class OpmDbRepositoryTest
    {
        private readonly IOpmRepository _repository = new OpmDbRepository();

        [Test]
        public void GetTest()
        {
            Opm opm;
            Assert.IsTrue(_repository.TryGetOpm(new EanEicCode("859182400205312606"), out opm));
            Assert.AreEqual("859182400205312606", opm.Code.Code);
        }
    }
}
