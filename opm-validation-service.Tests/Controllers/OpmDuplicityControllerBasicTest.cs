﻿using System.IO;
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
    /// </summary>
    [TestFixture]
    public class OpmDuplicityControllerBasicTest
    {
        private OpmDuplicityController _controller;
        private const string PositiveTestData = "OpmRepoSampleData.csv";
        private const string NegativeTestData = "OpmRepoSampleNegativeData.csv";

        [SetUp]
        public void SetUp()
        {
            Mock<IEanEicCheckerHttpClient> mockClient = new Mock<IEanEicCheckerHttpClient>();
            mockClient.Setup(c => c.Post(It.IsAny<EanEicCode>())).Returns(new CheckResult(CheckResultCode.EanOk));

            IOpmRepository opmRepository = new OpmInMemoryRepository();

            OpmRepoFiller.Fill(opmRepository, PositiveTestData);
            Mock<IUserAccessService> userAccessServiceMock = new Mock<IUserAccessService>();
            userAccessServiceMock.Setup(m => m.TryAccess(It.IsAny<IUser>())).Returns(true);
            IIdentityManagement identityManagement = new IdentityManagementMock();
            IOpmVerificator opmVerificator = new OpmVerificator(identityManagement, mockClient.Object, opmRepository, userAccessServiceMock.Object);

            _controller = new OpmDuplicityController(opmVerificator)
                {
                    Request = new HttpRequestMessage(),
                    Configuration = new HttpConfiguration()
                };
        }

        [Test]
        public void Get()
        {
            TestUsingExternalData(false, null);
            TestUsingExternalData(true, null);
        }

        [Test]
        public void Get_Returns_401_For_Wrong_Token()
        {
            TestUsingExternalData(false, "valid");
            TestUsingExternalData(true, "valid");
            try
            {
                TestUsingExternalData(false, "invalid");
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.Response.StatusCode, HttpStatusCode.Unauthorized);
                return;
            }
            Assert.Fail("Test failed. Expected HTTP Status Code 401.");
        }

        private void TestUsingExternalData(bool isPositive, string token)
        {
            string path = isPositive ? PositiveTestData : NegativeTestData;
            using (StreamReader sr = new StreamReader(path))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    if (token != null)
                    {
                        Assert.IsFalse(_controller.Get(currentLine, token).Result ^ isPositive);
                    }
                    else
                    {
                        Assert.IsFalse(_controller.Get(currentLine).Result ^ isPositive);
                    }
                }
            }
        }
    }
}
