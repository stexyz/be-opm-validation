using NUnit.Framework;
using opm_validation_service.Models;
using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence {
    [TestFixture]
    public class OpmRepositoryTest {
        readonly OpmInMemoryRepository _repository = new OpmInMemoryRepository();

        readonly EanEicCode _validCode = new EanEicCode("859182400123456789");

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
