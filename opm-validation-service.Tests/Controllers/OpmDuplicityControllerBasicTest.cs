using System.Net;
using System.Web.Http;
using NUnit.Framework;
using opm_validation_service.Models;

namespace opm_validation_service.Tests.Controllers {
    /// <summary>
    /// Tests only that the logic works without auth and user access limits.
    /// Two token values with some meaning - "valid" and "depleated".
    /// </summary>
    [TestFixture]
    public class OpmDuplicityControllerBasicTest : OpmDuplicityControllerTestBase
    {

        [Test]
        public void Get_With_Valid_Token_In_Uri()
        {
            TestUsingExternalDataWithTokenAsUriParam(false, "valid");
            TestUsingExternalDataWithTokenAsUriParam(true, "valid");
        }

        [Test]
        public void Get_With_Valid_Token_In_Cookie()
        {
            TestUsingExternalData(false, "valid");
            TestUsingExternalData(true, "valid");
        }

        [Test]
        public void Get_Returns_400_For_Wrong_Code()
        {
            try {
                Controller.Get("invalid", "valid");
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 400."); 
        }

        [Test]
        public void Get_Returns_400_For_Wrong_Code_With_Cookie() {
            try
            {
                // set the sso token (mock takes 'valid' as valid)
                Controller.Request.Headers.Add("Cookie", SsoCookieName + "=valid");
                Controller.Get("invalid");
            }
            catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 400.");
        }

        [Test]
        public void Get_Returns_401_For_Wrong_Cookie_Token() {
            try {
                // set the sso token (mock takes 'valid' as valid)
                Controller.Request.Headers.Add("Cookie", SsoCookieName + "=NotValidToken");
                OpmVerificationResult result = Controller.Get("invalid");
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.Unauthorized, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 401.");
        }

        [Test]
        public void Get_Returns_401_For_Wrong_Token() {
            TestUsingExternalDataWithTokenAsUriParam(false, "valid");
            TestUsingExternalDataWithTokenAsUriParam(true, "valid");
            try {
                TestUsingExternalDataWithTokenAsUriParam(false, "invalid");
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.Unauthorized, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 401.");
        }

        [Test]
        public void Get_Returns_401_For_Missing_Cookie_Token()
        {
            try {
                // no cookie set
                Controller.Get("Anything..");
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.Unauthorized, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 401.");
        }

        [Test]
        public void Get_Returns_401_For_Null_Token() {
            try {
                Controller.Get("Anything..", null);
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.Unauthorized, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 401.");
        }        
    }
}
