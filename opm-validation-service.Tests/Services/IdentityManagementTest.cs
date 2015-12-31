﻿using NUnit.Framework;
using opm_validation_service.Models;
using opm_validation_service.Services;

namespace opm_validation_service.Tests.Services {
    [TestFixture]
    public class IdentityManagementTest {
        //TODO SP: use Unity to setup tests
        readonly string _idmUrl = System.Configuration.ConfigurationManager.AppSettings["IdmUrl"];

        [Test]
        public void BasicImTest() {

            IdentityManagement im = new IdentityManagement(_idmUrl);

            string token = im.Login("t5734", "Lcii9lvy");
            const string badToken = "AAABB123123123DFSDFsdfsdf123123dsfsdf123SDFSDF4.*AAJTSQACMDE.*";

            bool userValid = im.ValidateUser(badToken);
            Assert.IsFalse(userValid);

            IUser user = im.GetUserInfo(badToken);
            Assert.IsNull(user);

            userValid = im.ValidateUser(token);
            Assert.IsTrue(userValid);

            IUser user2 = im.GetUserInfo(token);
            Assert.NotNull(user2);
            Assert.AreEqual(user2.Id, "t5734");
        }
    }
}
