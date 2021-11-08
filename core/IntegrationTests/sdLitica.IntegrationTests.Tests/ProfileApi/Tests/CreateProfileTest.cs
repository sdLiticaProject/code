using System;
using System.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.ProfileApi.Helpers;
using sdLitica.IntegrationTests.Tests.ProfileApi.TestData;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Tests
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

            Given
                .NewUserData(profile)
                .When
                .CreateUserRequestIsSend()
                .Then
                .LastRequestSuccessful();

            Given
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = profile.Email,
                    Password = profile.Password
                })
                .When
                .LoginRequestIsSend()
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(profile)
                .LogoutIfSessionTokenIsPresent();
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestCreateNewProfileWithGuid()
        {
            var profile = new TestUserModel
            {
                Email = $"{Guid.NewGuid().ToString()}@example.com",
                Password = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
            };

            Given
                .NewUserData(profile)
                .When
                .CreateUserRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.BadRequest);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        [TestCaseSource(typeof(CreateNewProfileData), nameof(CreateNewProfileData.NegativeNewProfileData))]
        public void BaseNegativeCreateUserTest(TestUserModel profile)
        {
            Given
                .NewUserData(profile)
                .When
                .CreateUserRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.BadRequest);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        [TestCaseSource(typeof(CreateNewProfileData), nameof(CreateNewProfileData.PositiveNewProfileData))]
        public void BasePositiveCreateUserTest(TestUserModel profile)
        {
            Given
                .NewUserData(profile)
                .When
                .CreateUserRequestIsSend()
                .Then
                .LastRequestSuccessful();

            Given
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = profile.Email,
                    Password = profile.Password
                })
                .When
                .LoginRequestIsSend()
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(profile)
                .LogoutIfSessionTokenIsPresent();
        }
    }
}
