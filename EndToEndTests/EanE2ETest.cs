using EndToEndTests.HttpClient;
using NUnit.Framework;
using opm_validation_service.Models;
using opm_validation_service.Services;

namespace EndToEndTests
{
    [TestFixture]
    public class EanE2ETest
    {
        private OpmVerificationHttpClient httpClient;
        private IdentityManagement identityManagement;

        [SetUp]
        public void SetUp() {
            httpClient = new OpmVerificationHttpClient(@"http://192.168.88.190:9876/");
            identityManagement = new IdentityManagement(@"https://am-proxytest.bohemiaenergy.cz/opensso/identity/");
        }

        //TODO SP: inherit from the controller tests
        [Test]
        public void Get_With_Inline_Token_Returns_True()
        {
            string token = identityManagement.Login("t5734", "Lcii9lvy");

            OpmVerificationResult res = httpClient.GetWithInlineToken("859182400100447106", token);
        
            Assert.AreEqual(true, res.Result);
        }

        [Test]
        public void Get_With_Inline_Token_Returns_False() {
            string token = identityManagement.Login("t5734", "Lcii9lvy");

            OpmVerificationResult res = httpClient.GetWithInlineToken("859182400741757329", token);

            Assert.AreEqual(false, res.Result);
        }

    }
}
