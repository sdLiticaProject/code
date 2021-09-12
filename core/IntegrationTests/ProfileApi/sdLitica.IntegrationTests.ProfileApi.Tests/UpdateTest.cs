using System;
using System.Net;
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
            var oldProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            oldProfile.FirstName = TestStringHelper.RandomLatinString();
            oldProfile.LastName = TestStringHelper.RandomLatinString();

            Facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = oldProfile.FirstName,
                LastName = oldProfile.LastName
            }).AssertSuccess();

            var newProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestDoubleUpdate()
        {
            var oldProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            oldProfile.FirstName = TestStringHelper.RandomLatinString();
            oldProfile.LastName = TestStringHelper.RandomLatinString();

            Facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = oldProfile.FirstName,
                LastName = oldProfile.LastName
            }).AssertSuccess();

            var newProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");

            oldProfile = newProfile;

            Facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = oldProfile.FirstName,
                LastName = oldProfile.LastName
            }).AssertSuccess();

            newProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateWithSessionEmpty()
        {
            Facade.PostUpdateProfileNames(string.Empty, new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString()
            }).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateOnlyFirstName()
        {
            var oldProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = String.Empty,
            }).AssertError(HttpStatusCode.BadRequest);

            var newProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateOnlyLastName()
        {
            var oldProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                LastName = TestStringHelper.RandomLatinString(),
                FirstName = String.Empty,
            }).AssertError(HttpStatusCode.BadRequest);

            var newProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateAllNulls()
        {
            var oldProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                LastName = null,
                FirstName = null,
            }).AssertError(HttpStatusCode.BadRequest);

            var newProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateExpiredSession()
        {
            var oldProfile = Facade.GetMe(Session).AssertSuccess().MapAndLog<TestUserModel>();

            Facade.PostLogout(Session);

            Facade.PostUpdateProfileNames(Session, new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString()
            }).AssertError(HttpStatusCode.Unauthorized);

            var session = Facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess().GetTokenFromResponse();

            var newProfile = Facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();
            
            Facade.PostLogout(session);

            Assert.That(JObject.FromObject(newProfile), Is.EqualTo(JObject.FromObject(oldProfile)),
                $"Expected to have '{oldProfile}' profile after update, but found '{newProfile}'");
        }
    }
}