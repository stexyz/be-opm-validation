using System.Net.Http;
using System.Web;
using System.Web.Http;
using NUnit.Framework;
using opm_validation_service.Controllers;
using opm_validation_service.Persistence;
using opm_validation_service.Services;

namespace opm_validation_service.Tests.Controllers {

    /// <summary>
    /// Tests only that the logic works without auth and user access limits.
    /// </summary>
    [TestFixture]
    public class OpmDuplicityControllerBasicTest
    {
        private IOpmVerificator opmVerificator;
        private OpmDuplicityController controller;

        [SetUp]
        public void SetUp()
        {
            string idmUrl = System.Configuration.ConfigurationManager.AppSettings["IdmUrl"];
            IIdentityManagement identityManagement = new IdentityManagement(idmUrl);

            string eanEicCheckerUrl = System.Configuration.ConfigurationManager.AppSettings["EanEicCheckerUrl"];
            IEanEicCheckerHttpClient eanEicCheckerHttpClient = new EanEicCheckerHttpClient(eanEicCheckerUrl);

            IOpmRepository opmRepository = new OpmInMemoryRepository();
            string path = "OpmRepoSampleData.csv";
            OpmRepoFiller.Fill(opmRepository, path);

            IUserAccessService userAccessService = new UserAccessMockService();

            opmVerificator = new OpmVerificator(identityManagement, eanEicCheckerHttpClient, opmRepository, userAccessService);

            // Arrange
            controller = new OpmDuplicityController(opmVerificator);

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }

        [Test]
        public void Get()
        {
            Assert.IsFalse(controller.Get("859182400123456789").Result);
            Assert.IsFalse(controller.Get("859182400488143676").Result);
            Assert.IsTrue(controller.Get("859182400206514634").Result);
            Assert.IsTrue(controller.Get("859182400204236002").Result);
        }
    }
}
