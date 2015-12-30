using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Moq;
using NUnit.Framework;
using opm_validation_service.Controllers;
using opm_validation_service.Models;
using opm_validation_service.Persistence;
using opm_validation_service.Services;
using opm_validation_service.Tests.Services;

namespace opm_validation_service.Tests.Controllers {

    /// <summary>
    /// Tests only that the logic works without auth and user access limits.
    /// Two token values with some meaning - "valid" and "depleated".
    /// </summary>
    [TestFixture]
    public class OpmDuplicityControllerBasicTest
    {
        private OpmDuplicityController _controller;
        private readonly string _ssoCookieName = System.Configuration.ConfigurationManager.AppSettings["ssoCookieName"];
        private const string PositiveTestData = "OpmRepoSampleData.csv";
        private const string NegativeTestData = "OpmRepoSampleNegativeData.csv";

        [SetUp]
        public void SetUp()
        {
            Mock<IEanEicCheckerHttpClient> mockClient = new Mock<IEanEicCheckerHttpClient>();
            //for "invalid" ean code returns invalid, ean ok otherwise
            mockClient.Setup(c => c.Post(It.IsAny<EanEicCode>()))
                      .Returns(
                          (EanEicCode code) =>
                          code.Code == "invalid"
                              ? new CheckResult(CheckResultCode.EanInvalidCheckCharacter)
                              : new CheckResult(CheckResultCode.EanOk));

            IOpmRepository opmRepository = new OpmInMemoryRepository();
            OpmRepoFiller.Fill(opmRepository, PositiveTestData);

            Mock<IUserAccessService> userAccessServiceMock = new Mock<IUserAccessService>();
            userAccessServiceMock.Setup(m => m.TryAccess(It.IsAny<IUser>())).Returns((IUser u) => u .Id != "depleated");
            
            IIdentityManagement identityManagement = new IdentityManagementMock();
            IOpmVerificator opmVerificator = new OpmVerificator(identityManagement, mockClient.Object, opmRepository, userAccessServiceMock.Object);

            _controller = new OpmDuplicityController(opmVerificator)
                {
                    Request = new HttpRequestMessage(),
                    Configuration = new HttpConfiguration()
                };
        }

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
                _controller.Get("invalid", "valid");
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
                _controller.Request.Headers.Add("Cookie", _ssoCookieName + "=valid");
                _controller.Get("invalid");
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
                _controller.Request.Headers.Add("Cookie", _ssoCookieName + "=NotValidToken");
                OpmVerificationResult result = _controller.Get("invalid");
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
                _controller.Get("Anything..");
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.Unauthorized, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 401.");
        }

        [Test]
        public void Get_Returns_401_For_Null_Token() {
            try {
                _controller.Get("Anything..", null);
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.Unauthorized, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 401.");
        }
        
        [Test]
        public void Get_Returns_403_For_Depleated_User()
        {
            try {
                _controller.Get("Anything..", "depleated");
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 403.");
        }

        [Test]
        public void Get_Returns_403_For_Depleated_User_Cookie_Version()
        {
            try {
                _controller.Request.Headers.Add("Cookie", _ssoCookieName + "=depleated");
                _controller.Get("Anything..");
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 403.");
        }

        private void TestUsingExternalData(bool isPositive, string token) {
            string path = isPositive ? PositiveTestData : NegativeTestData;
            using (StreamReader sr = new StreamReader(path)) {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    _controller.Request.Headers.Add("Cookie", _ssoCookieName + "=" + token);
                    Assert.IsFalse(_controller.Get(currentLine).Result ^ isPositive);
                }
            }
        }

        private void TestUsingExternalDataWithTokenAsUriParam(bool isPositive, string token)
        {
            string path = isPositive ? PositiveTestData : NegativeTestData;
            using (StreamReader sr = new StreamReader(path))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    Assert.IsFalse(_controller.Get(currentLine, token).Result ^ isPositive);
                }
            }
        }
    }
}
