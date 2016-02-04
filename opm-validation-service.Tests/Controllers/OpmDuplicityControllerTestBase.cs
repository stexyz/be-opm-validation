using System;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using Moq;
using NUnit.Framework;
using opm_validation_service.Controllers;
using opm_validation_service.Models;
using opm_validation_service.Persistence;
using opm_validation_service.Services;
using opm_validation_service.Tests.Services;

namespace opm_validation_service.Tests.Controllers
{
    public abstract class OpmDuplicityControllerTestBase
    {
        protected OpmDuplicityController Controller;
        protected readonly string SsoCookieName = System.Configuration.ConfigurationManager.AppSettings["ssoCookieName"];
        private const string PositiveTestData = "OpmRepoSampleData.csv";
        private const string NegativeTestData = "OpmRepoSampleNegativeData.csv";
        protected IEanEicCheckerHttpClient EanEicCheckerHttpClient;
        protected IOpmRepository OpmRepository;

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
            
            EanEicCheckerHttpClient = mockClient.Object;

            OpmRepository = new OpmInMemoryRepository();
            OpmRepoFiller.Fill(OpmRepository, PositiveTestData);
            IUserAccessService userAccessService = new UserAccessService(new UserAccessInMemoryRepository(),
                                                                         new TimeSpan(0, 1, 0), 999);

            IIdentityManagement identityManagement = new IdentityManagementMock();
            IOpmVerificator opmVerificator = new OpmVerificator(identityManagement, mockClient.Object, OpmRepository,
                                                                userAccessService);

            Controller = new OpmDuplicityController(opmVerificator, userAccessService)
                {
                    Request = new HttpRequestMessage(),
                    Configuration = new HttpConfiguration()
                };
        }

        protected void TestUsingExternalData(bool isPositive, string token)
        {
            string path = isPositive ? PositiveTestData : NegativeTestData;
            using (StreamReader sr = new StreamReader(path))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    Controller.Request.Headers.Add("Cookie", SsoCookieName + "=" + token);
                    Assert.IsFalse(Controller.Get(currentLine).Result ^ isPositive);
                }
            }
        }

        protected void TestUsingExternalDataWithTokenAsUriParam(bool isPositive, string token)
        {
            string path = isPositive ? PositiveTestData : NegativeTestData;
            using (StreamReader sr = new StreamReader(path))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    Assert.IsFalse(Controller.Get(currentLine, token).Result ^ isPositive);
                }
            }
        }
    }
}