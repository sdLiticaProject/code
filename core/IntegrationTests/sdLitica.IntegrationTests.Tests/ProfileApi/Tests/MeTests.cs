using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Helpers;
using sdLitica.IntegrationTests.Tests.ProfileApi.TestData;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Tests
{
    public class MeTests : ProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeForConfigUserMe()
        {
            var session = Facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess().GetTokenFromResponse();

            var newProfile = Facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();
            Facade.PostLogout(session).AssertSuccess();

            Assert.That(newProfile.Email, Is.EqualTo(Configuration.UserName));
            Assert.That(newProfile.Password, Is.Null);
            Assert.That(newProfile.Id, Is.Not.Null);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeForNewUserMe()
        {
            var profile = new TestUserModel
            {
                Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                Password = TestStringHelper.RandomLatinString(),
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString(),
            };

            Facade.PostCreateNewProfile(profile).AssertSuccess();

            var session = Facade.PostLogin(new TestLoginModel
            {
                Email = profile.Email,
                Password = profile.Password
            }).AssertSuccess().GetTokenFromResponse();

            var newProfile = Facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();
            Facade.PostLogout(session).AssertSuccess();

            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(profile, newProfile));
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeForNewUserAfterUpdateMe()
        {
            var profile = new TestUserModel
            {
                Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                Password = TestStringHelper.RandomLatinString(),
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString(),
            };

            var createdProfile = Facade.PostCreateNewProfile(profile).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(profile, createdProfile));

            var session = Facade.PostLogin(new TestLoginModel
            {
                Email = profile.Email,
                Password = profile.Password
            }).AssertSuccess().GetTokenFromResponse();

            var newProfile = Facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(profile, newProfile));

            var updateProfile = new TestUserUpdateModel
                {FirstName = TestStringHelper.RandomLatinString(), LastName = TestStringHelper.RandomLatinString()};

            Facade.PostUpdateProfileNames(session, updateProfile).AssertSuccess();

            profile.FirstName = updateProfile.FirstName;
            profile.LastName = updateProfile.LastName;

            var updatedProfile = Facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();

            Facade.PostLogout(session).AssertSuccess();

            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(profile, updatedProfile));
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(MeData), nameof(MeData.NegativeMyProfileData))]
        public void BaseNegativeMyUserTest(string session)
        {
            Facade.GetMe(session).AssertError(HttpStatusCode.Unauthorized);
        }
    }
}