using System;
using System.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tests.TestData;
using sdLitica.IntegrationTests.ProfileApi.Tools;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.RestApiTestBase;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    public class ProfileTest : ProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeCreateNewProfile()
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

            Logger.Information($"Got new profile: {newProfile}");

            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(profile, newProfile));
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestCreateNewProfileWithGuid()
        {
            var profile = new TestUserModel
            {
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                Password = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
            };

            var createdProfile = JObject
                .Parse(_facade.PostCreateNewProfile(profile).AssertSuccess().Content.ReadAsStringAsync().Result)
                .ToObject<TestUserModel>();

            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(profile, createdProfile));

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
        [Category(nameof(TestCategories.PriorityMedium))]
        [TestCaseSource(typeof(CreateNewProfileData), nameof(CreateNewProfileData.NegativeNewProfileData))]
        public void BaseNegativeCreateUserTest(TestUserModel profile)
        {
            _facade.PostCreateNewProfile(profile).AssertError(HttpStatusCode.BadRequest);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        [TestCaseSource(typeof(CreateNewProfileData), nameof(CreateNewProfileData.PositiveNewProfileData))]
        public void BasePositiveCreateUserTest(TestUserModel profile)
        {
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
    }
}