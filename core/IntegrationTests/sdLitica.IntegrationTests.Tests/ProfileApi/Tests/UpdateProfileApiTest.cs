using System.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.CommonTestData;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Tests
{
    public class UpdateProfileApiTest : BaseAuthorizedProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeUpdate()
        {
            var oldProfile = Given
                .UserSession(Session)
                .When
                .GetCurrentUserRequestIsSend()
                .GetResultData<TestUserModel>(BddKeyConstants.CurrentUserResponse);

            var updateModel = new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString()
            };
            oldProfile.ApplyUpdate(updateModel);

            Given
                .UserSession(Session)
                .When
                .UpdateUserRequestIsSend(updateModel)
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(oldProfile);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestDoubleUpdate()
        {
            var oldProfile = Given
                .UserSession(Session)
                .When
                .GetCurrentUserRequestIsSend()
                .GetResultData<TestUserModel>(BddKeyConstants.CurrentUserResponse);

            var updateModel = new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString()
            };
            oldProfile.ApplyUpdate(updateModel);

            Given
                .UserSession(Session)
                .When
                .UpdateUserRequestIsSend(updateModel)
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(oldProfile);
            
            updateModel = new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = TestStringHelper.RandomLatinString()
            };
            oldProfile.ApplyUpdate(updateModel);
            
            Given
                .UserSession(Session)
                .When
                .UpdateUserRequestIsSend(updateModel)
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(oldProfile);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
        public void TestUpdateWithSessionEmpty(string session)
        {
            Given
                .UserSession(session)
                .When
                .UpdateUserRequestIsSend(new TestUserUpdateModel
                {
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString()
                })
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateOnlyFirstName()
        {
            var oldProfile = Given
                .UserSession(Session)
                .When
                .GetCurrentUserRequestIsSend()
                .GetResultData<TestUserModel>(BddKeyConstants.CurrentUserResponse);

            var updateModel = new TestUserUpdateModel
            {
                FirstName = TestStringHelper.RandomLatinString(),
                LastName = string.Empty
            };

            Given
                .UserSession(Session)
                .When
                .UpdateUserRequestIsSend(updateModel)
                .WithCode(HttpStatusCode.BadRequest)
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(oldProfile);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateOnlyLastName()
        {
            var oldProfile = Given
                .UserSession(Session)
                .When
                .GetCurrentUserRequestIsSend()
                .GetResultData<TestUserModel>(BddKeyConstants.CurrentUserResponse);

            var updateModel = new TestUserUpdateModel
            {
                FirstName = string.Empty,
                LastName = TestStringHelper.RandomLatinString()
            };

            Given
                .UserSession(Session)
                .When
                .UpdateUserRequestIsSend(updateModel)
                .WithCode(HttpStatusCode.BadRequest)
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(oldProfile);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateAllNulls()
        {
            var oldProfile = Given
                .UserSession(Session)
                .When
                .GetCurrentUserRequestIsSend()
                .GetResultData<TestUserModel>(BddKeyConstants.CurrentUserResponse);

            var updateModel = new TestUserUpdateModel
            {
                FirstName = null,
                LastName = null
            };

            Given
                .UserSession(Session)
                .When
                .UpdateUserRequestIsSend(updateModel)
                .WithCode(HttpStatusCode.BadRequest)
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(oldProfile);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestUpdateExpiredSession()
        {
            var oldProfile = Given
                .UserSession(Session)
                .When
                .GetCurrentUserRequestIsSend()
                .GetResultData<TestUserModel>(BddKeyConstants.CurrentUserResponse);
            
            var updateModel = new TestUserUpdateModel
            {
                FirstName = null,
                LastName = null
            };

            Given
                .DefaultUserLoginCredentials()
                .UserSession(Session)
                .When
                .LogoutRequestIsSend()
                .UpdateUserRequestIsSend(updateModel)
                .WithCode(HttpStatusCode.Unauthorized)
                .LoginRequestIsSend()
                .GetCurrentUserRequestIsSend()
                .Then
                .CurrentUserIsEqualTo(oldProfile)
                .LogoutIfSessionTokenIsPresent();
        }
    }
}