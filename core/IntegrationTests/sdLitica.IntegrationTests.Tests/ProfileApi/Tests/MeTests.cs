using System;
using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.CommonTestData;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.ProfileApi.Helpers;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
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
            new GivenStatement(Logger)
                .DefaultUserLoginCredentials()
                .When
                .LoginRequestIsSend()
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualToExpected()
                .LogoutIfSessionTokenIsPresent();
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeForNewUserMe()
        {
            var userModel = new TestUserModel()
            {
                Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                Password = TestStringHelper.RandomLatinString(),
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString(),
            };

            new GivenStatement(Logger)
                .NewUserData(userModel)
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = userModel.Email,
                    Password = userModel.Password,
                })
                .When
                .CreateUserRequestIsSend()
                .LoginRequestIsSend()
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(userModel)
                .LogoutIfSessionTokenIsPresent();
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeForNewUserAfterUpdateMe()
        {
            var userModel = new TestUserModel()
            {
                Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                Password = TestStringHelper.RandomLatinString(),
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString(),
            };
            
            var updateModel = new TestUserUpdateModel
                {FirstName = TestStringHelper.RandomLatinString(), LastName = TestStringHelper.RandomLatinString()};

            var whenLoggedIn = new GivenStatement(Logger)
                .NewUserData(userModel)
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = userModel.Email,
                    Password = userModel.Password,
                })
                .When
                .CreateUserRequestIsSend()
                .LoginRequestIsSend();
            
            whenLoggedIn
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(userModel);
            
            whenLoggedIn
                .UpdateUserRequestIsSend(updateModel)
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(userModel.ApplyUpdate(updateModel))
                .LogoutIfSessionTokenIsPresent();
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
        public void BaseNegativeMyUserTest(string session)
        {
            new GivenStatement(Logger)
                .UserSession(session)
                .When
                .GetCurrentUserRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }
    }
}