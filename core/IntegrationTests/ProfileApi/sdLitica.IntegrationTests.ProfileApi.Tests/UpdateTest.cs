using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.RestApiTestBase;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    public class UpdateTest : BaseAuthorizedTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeUpdate()
        {
            var oldProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            oldProfile.FirstName = TestStringHelper.RandomLatinString();
            oldProfile.LastName = TestStringHelper.RandomLatinString();

            _facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = oldProfile.FirstName,
                LastName = oldProfile.LastName
            }).AssertSuccess();

            var newProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestDoubleUpdate()
        {
            var oldProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            oldProfile.FirstName = TestStringHelper.RandomLatinString();
            oldProfile.LastName = TestStringHelper.RandomLatinString();

            _facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = oldProfile.FirstName,
                LastName = oldProfile.LastName
            }).AssertSuccess();

            var newProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");

            oldProfile = newProfile;

            _facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = oldProfile.FirstName,
                LastName = oldProfile.LastName
            }).AssertSuccess();

            newProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateWithSessionEmpty()
        {
            _facade.PostUpdateProfileNames(string.Empty, new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString()
            }).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateOnlyFirstName()
        {
            var oldProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            _facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = String.Empty,
            }).AssertError(HttpStatusCode.BadRequest);

            var newProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateOnlyLastName()
        {
            var oldProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            _facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                LastName = TestStringHelper.RandomLatinString(),
                FirstName = String.Empty,
            }).AssertError(HttpStatusCode.BadRequest);

            var newProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateAllNulls()
        {
            var oldProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            _facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                LastName = null,
                FirstName = null,
            }).AssertError(HttpStatusCode.BadRequest);

            var newProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateExpiredSession()
        {
            var oldProfile = _facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            _facade.PostLogout(Session);

            _facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString()
            }).AssertError(HttpStatusCode.Unauthorized);

            var session = _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess().GetTokenFromResponse();

            var newProfile = _facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();
            
            _facade.PostLogout(session);

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }
    }
}