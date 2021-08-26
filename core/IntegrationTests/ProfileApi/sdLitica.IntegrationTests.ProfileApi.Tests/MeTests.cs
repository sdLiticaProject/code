using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tests.TestData;
using sdLitica.IntegrationTests.ProfileApi.Tools;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.RestApiTestBase;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    public class MeTests : ProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeForConfigUserMe()
        {
            var session = _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess().GetTokenFromResponse();

            var newProfile = _facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();
            _facade.PostLogout(session).AssertSuccess();

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

            _facade.PostCreateNewProfile(profile).AssertSuccess();

            var session = _facade.PostLogin(new TestLoginModel
            {
                Email = profile.Email,
                Password = profile.Password
            }).AssertSuccess().GetTokenFromResponse();

            var newProfile = _facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();
            _facade.PostLogout(session).AssertSuccess();

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

            var createdProfile = _facade.PostCreateNewProfile(profile).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(profile, createdProfile));

            var session = _facade.PostLogin(new TestLoginModel
            {
                Email = profile.Email,
                Password = profile.Password
            }).AssertSuccess().GetTokenFromResponse();

            var newProfile = _facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();

            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(profile, newProfile));

            var updateProfile = new TestUserUpdateModel
                {FirstName = TestStringHelper.RandomLatinString(), LastName = TestStringHelper.RandomLatinString()};

            _facade.PostUpdateProfileNames(session, updateProfile).AssertSuccess();

            profile.FirstName = updateProfile.FirstName;
            profile.LastName = updateProfile.LastName;

            var updatedProfile = _facade.GetMe(session).AssertSuccess().MapAndLog<TestUserModel>();

            _facade.PostLogout(session).AssertSuccess();

            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(profile, updatedProfile));
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(MeData), nameof(MeData.NegativeMyProfileData))]
        public void BaseNegativeMyUserTest(string session)
        {
            _facade.GetMe(session).AssertError(HttpStatusCode.Unauthorized);
        }
    }
}