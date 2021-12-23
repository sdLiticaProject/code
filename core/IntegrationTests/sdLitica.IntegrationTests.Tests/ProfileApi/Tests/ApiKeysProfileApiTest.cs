using System.Linq;
using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.CommonTestData;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Tests
{
    public class ApiKeysProfileApiTest : BaseAuthorizedProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeApiKeys()
        {
            var newKeyDescription = TestStringHelper.RandomLatinString();

            var userKeys =
                Given
                    .UserSession(Session)
                    .NewApiKey(new TestUserApiKeyJsonEntity
                    {
                        Description = newKeyDescription
                    })
                    .When
                    .CreateNewApiKeyRequestIsSend()
                    .GetApiKeysRequestIsSend()
                    .Then
                    .UsersApiKeysContainGiven()
                    .GetResultData<TestApiKeysList>(BddKeyConstants.UserApiKeys);

            var apiKeyId = userKeys.Entities.First(e => e.Description.Equals(newKeyDescription)).Id;
            
            Given
                .UserSession(Session)
                .ApiKeyToRemove(apiKeyId)
                .When
                .DeleteApiKeyRequestIsSend()
                .GetApiKeysRequestIsSend()
                .Then
                .UsersApiKeysDoNotContainGiven();
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
        public void BaseNegativeGetApiKeysTest(string session)
        {
            Given
                .UserSession(session)
                .When
                .GetApiKeysRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
        public void BaseNegativePostApiKeyTest(string session)
        {
            Given
                .UserSession(session)
                .NewApiKey(new TestUserApiKeyJsonEntity())
                .When
                .CreateNewApiKeyRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
        public void BaseNegativeDeleteApiKeyTest(string session)
        {
            Given
                .UserSession(session)
                .ApiKeyToRemove("some-key")
                .When
                .DeleteApiKeyRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }
    }
}