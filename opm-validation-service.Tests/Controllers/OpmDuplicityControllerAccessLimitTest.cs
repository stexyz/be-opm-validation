using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;
using opm_validation_service.Controllers;
using opm_validation_service.Persistence;
using opm_validation_service.Services;
using opm_validation_service.Tests.Services;

namespace opm_validation_service.Tests.Controllers
{

    /// <summary>
    /// Tests only that the logic works without auth and user access limits.
    /// Two token values with some meaning - "valid" and "depleated".
    /// </summary>
    [TestFixture]
    public class OpmDuplicityControllerAccessLimitTest : OpmDuplicityControllerTestBase
    {
        //TODO SP: add the "cookie version" get() tests
        private const int MaxUserLimit = 5;

        [SetUp]
        public new void SetUp()
        {
            base.SetUp();
            IUserAccessService userAccessService = new UserAccessService(new UserAccessInMemoryRepository(),
                                                                         new TimeSpan(0, 1, 0), MaxUserLimit);

            IIdentityManagement identityManagement = new IdentityManagementMock();
            IOpmVerificator opmVerificator = new OpmVerificator(identityManagement, EanEicCheckerHttpClient,
                                                                OpmRepository,
                                                                userAccessService);

            Controller = new OpmDuplicityController(opmVerificator)
                {
                    Request = new HttpRequestMessage(),
                    Configuration = new HttpConfiguration()
                };
        }

        [Test]
        public void Get_Returns_403_After_User_Limit_Is_Depleated()
        {
            Test_Get_Until_Depleated("valid", false);
        }

        [Test]
        public void Get_Still_Works_For_Another_User_After_Ones_User_Limit_Is_Depleated()
        {
            Test_Get_Until_Depleated("valid", false);
            Test_Get_Until_Depleated("valid2", false);
        }

        [Test]
        public void Get_Returns_403_After_User_Limit_Is_Depleated_Cookie() {
            Test_Get_Until_Depleated("valid", true);
        }

        [Test]
        public void Get_Still_Works_For_Another_User_After_Ones_User_Limit_Is_Depleated_Cookie() {
            Test_Get_Until_Depleated("valid", true);
            Test_Get_Until_Depleated("valid2", true);
        }

        private void Test_Get_Until_Depleated(string token, bool cookieVersion) {
            // the limit is 5, so all 5 calls have to pass
            for (int i = 0; i < MaxUserLimit; i++)
            {
                ControllerGet(cookieVersion, token);
            }

            try {
                // the 6th call should fail with HTTP 403
                ControllerGet(cookieVersion, token);
            } catch (HttpResponseException e) {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 403.");
        }

        private void ControllerGet(bool withCookie, string token)
        {
            if (withCookie)
            {
                Controller.Request.Headers.Remove("Cookie");
                Controller.Request.Headers.Add("Cookie", SsoCookieName + "=" + token);
                Controller.Get("Anything");
            }
            else
            {
                Controller.Get("Anything", token);
            }
        }
    }
}
