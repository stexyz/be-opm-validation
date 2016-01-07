using NUnit.Framework;
using opm_validation_service.Models;
using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence {
    [TestFixture]
    public abstract class OpmRepositoryTestBase
    {
        protected abstract IOpmRepository GetOpmRepository();
        private IOpmRepository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = GetOpmRepository();
        }

        readonly EanEicCode _validCode = new EanEicCode("859182400123456789");

        [Test]
        public void CrudTest() {
            _repository.TryAdd(new Opm(_validCode));
            Opm opm;
            Assert.IsTrue(_repository.TryGetOpm(_validCode, out opm));
            Assert.AreEqual(_validCode, opm.Code);

            _repository.TryRemoveOpm(opm.Code);
            Assert.IsFalse(_repository.TryGetOpm(_validCode, out opm));
        }

        [Test]
        public void Try_Get_Nonexistent_Opm()
        {
            Opm opm;
            Assert.IsFalse(_repository.TryGetOpm(new EanEicCode("Clearly nonexistent code #$%."), out opm));
        }

        [Test]
        public void Get_With_Null_Code()
        {
            Opm opm;
            Assert.IsFalse(_repository.TryGetOpm(null, out opm));
            Assert.IsNull(opm);
        }

        [Test]
        public void Add_Null()
        {
            Assert.IsFalse(_repository.TryAdd(null));
        }

        [Test]
        public void Remove_Null()
        {
            Assert.IsFalse(_repository.TryRemoveOpm(null));
        }

        [Test]
        public void Remove_Nonexistent() {
            Assert.IsFalse(_repository.TryRemoveOpm(new EanEicCode("Clearly nonexistent code #$%.")));
        }
    }
}
