﻿using System;
using NUnit.Framework;
using opm_validation_service.Models;
using opm_validation_service.Persistence;

namespace opm_validation_service.Tests.Peristence
{
    [TestFixture]
    public abstract class UserAccessRepositoryTestBase
    {
        protected abstract IUserAccessRepository CreateRepository();

        private IUserAccessRepository _repository;
        private readonly IUser _user = new User("userXYZ");
        private readonly IUser _user2 = new User("userXYZ2");

        [SetUp]
        public void Setup()
        {
            _repository = CreateRepository();
        }

        [Test]
        public void GetAndRecordTest()
        {
            Assert.AreEqual(0, _repository.GetUserAccessCount(_user, new TimeSpan(999, 0, 0)));
            for (int i = 0; i < 999; i++ ) {
                _repository.RecordAccess(_user, new EanEicCode(""));
                Assert.AreEqual(i + 1, _repository.GetUserAccessCount(_user, new TimeSpan(999, 0, 0)));
            }
        }

        [Test]
        public void RecordNullUser()
        {
            try
            {
                _repository.RecordAccess(null, new EanEicCode(""));
            }
            catch (ArgumentException)
            {
                return;
            }
            Assert.Fail("Expected argument exception");
        }

        [Test]
        public void RecordNullCode()
        {
            try
            {
                _repository.RecordAccess(_user, null);
            }
            catch (ArgumentException)
            {
                return;
            }
            Assert.Fail("Expected argument exception");
        }

        [Test]
        public void Get_Record_Returns_Zero_For_User_Without_Records()
        {
            Assert.AreEqual(0, _repository.GetUserAccessCount(_user, new TimeSpan(999, 0, 0)));
            _repository.RecordAccess(_user, new EanEicCode(""));
            Assert.AreEqual(1, _repository.GetUserAccessCount(_user, new TimeSpan(999, 0, 0)));
            Assert.AreEqual(0, _repository.GetUserAccessCount(_user2, new TimeSpan(999, 0, 0)));
        }

    }
}