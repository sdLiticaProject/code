using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tests.TestData;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.RestApiTestBase;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    public class ApiKeysTest : BaseAuthorizedTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeApiKeys()
        {
            var newKeyDescription = TestStringHelper.RandomLatinString();
            _facade.PostApiKeys(Session, new TestUserApiKeyJsonEntity
            {
                Description = newKeyDescription
            }).AssertSuccess();

            var result = _facade.GetApiKeys(Session).AssertSuccess().MapAndLog<List<TestUserApiKeyJsonEntity>>();
            Assert.That(newKeyDescription, Is.SubsetOf(result.Select(e => e.Description)));
            _facade.DeleteApiKey(Session, result.First(e => e.Description.Equals(newKeyDescription)).Id);

            result = _facade.GetApiKeys(Session).AssertSuccess().MapAndLog<List<TestUserApiKeyJsonEntity>>();
            Assert.That(newKeyDescription, Is.Not.SubsetOf(result.Select(e => e.Description)));
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(ApiKeysData), nameof(ApiKeysData.NegativeGetApiKeysSessionData))]
        public void BaseNegativeGetApiKeysTest(string session)
        {
            _facade.GetApiKeys(session).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(ApiKeysData), nameof(ApiKeysData.NegativePostApiKeysSessionData))]
        public void BaseNegativePostApiKeyTest(string session)
        {
            _facade.PostApiKeys(session, new TestUserApiKeyJsonEntity()).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(ApiKeysData), nameof(ApiKeysData.NegativeDeleteApiKeysSessionData))]
        public void BaseNegativeDeleteApiKeyTest(string session)
        {
            _facade.DeleteApiKey(session, "key-id").AssertError(HttpStatusCode.Unauthorized);
        }
    }
}