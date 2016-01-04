using NUnit.Framework;
using opm_validation_service.Models;
using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence {
    [TestFixture]
    public class OpmDbRepositoryTest
    {
        private readonly IOpmRepository _repository = new OpmDbRepository();

        readonly EanEicCode _validCode = new EanEicCode("859182400123456789");

        //TODO SP: use this test to test the DB repo as well.
        [Test]
        public void GetTest()
        {
            Opm opm;
            Assert.IsTrue(_repository.TryGetOpm(new EanEicCode("859182400205312606"), out opm));
            Assert.AreEqual("859182400205312606", opm.Code.Code);
        }


        [Test]
        public void CrudTest() {
            _repository.TryAdd(new Opm(_validCode));
            Opm opm;
            Assert.IsTrue(_repository.TryGetOpm(_validCode, out opm));

            _repository.TryRemoveOpm(opm.Code);
            Assert.IsFalse(_repository.TryGetOpm(_validCode, out opm));
        }
    }
}
