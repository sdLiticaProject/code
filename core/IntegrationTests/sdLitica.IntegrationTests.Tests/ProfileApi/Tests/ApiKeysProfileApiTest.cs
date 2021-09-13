using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
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

            var result = Facade.GetApiKeys(Session).AssertSuccess().MapAndLog<List<TestUserApiKeyJsonEntity>>();
            Assert.That(newKeyDescription, Is.SubsetOf(result.Select(e => e.Description)));
            Facade.DeleteApiKey(Session, result.First(e => e.Description.Equals(newKeyDescription)).Id);

            result = Facade.GetApiKeys(Session).AssertSuccess().MapAndLog<List<TestUserApiKeyJsonEntity>>();
            Assert.That(newKeyDescription, Is.Not.SubsetOf(result.Select(e => e.Description)));
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(ApiKeysData), nameof(ApiKeysData.NegativeGetApiKeysSessionData))]
        public void BaseNegativeGetApiKeysTest(string session)
        {
            Facade.GetApiKeys(session).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(ApiKeysData), nameof(ApiKeysData.NegativePostApiKeysSessionData))]
        public void BaseNegativePostApiKeyTest(string session)
        {
            Facade.PostApiKeys(session, new TestUserApiKeyJsonEntity()).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(ApiKeysData), nameof(ApiKeysData.NegativeDeleteApiKeysSessionData))]
        public void BaseNegativeDeleteApiKeyTest(string session)
        {
            Facade.DeleteApiKey(session, "key-id").AssertError(HttpStatusCode.Unauthorized);
        }
    }
}