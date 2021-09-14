using System.Linq;
using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.CommonTestData;
using sdLitica.IntegrationTests.Tests.ProfileApi.TestData;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Tests
{
    public class ApiKeysProfileApiTest : BaseAuthorizedProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeApiKeys()
        {
            var newKeyDescription = TestStringHelper.RandomLatinString();
            Facade.PostApiKeys(Session, new TestUserApiKeyJsonEntity
            {
                Description = newKeyDescription
            }).AssertSuccess();

            var result = Facade.GetApiKeys(Session).AssertSuccess().MapAndLog<TestApiKeysList>();
            Assert.That(result.Entities.Select(e => e.Description), Contains.Item(newKeyDescription));
            Facade.DeleteApiKey(Session, result.Entities.First(e => e.Description.Equals(newKeyDescription)).Id);

            result = Facade.GetApiKeys(Session).AssertSuccess().MapAndLog<TestApiKeysList>();
            Assert.False(result.Entities.Select(e => e.Description).Contains(newKeyDescription));
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
        public void BaseNegativeGetApiKeysTest(string session)
        {
            Facade.GetApiKeys(session).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
        public void BaseNegativePostApiKeyTest(string session)
        {
            Facade.PostApiKeys(session, new TestUserApiKeyJsonEntity()).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
        public void BaseNegativeDeleteApiKeyTest(string session)
        {
            Facade.DeleteApiKey(session, "key-id").AssertError(HttpStatusCode.Unauthorized);
        }
    }
}