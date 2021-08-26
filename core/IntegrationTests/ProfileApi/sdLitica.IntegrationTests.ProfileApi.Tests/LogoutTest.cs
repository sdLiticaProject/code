using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tests.TestData;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.RestApiTestBase;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    public class LogoutTest : ProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeLogout()
        {
            var session = _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess().GetTokenFromResponse();
            _facade.PostLogout(session).AssertSuccess();

            _facade.GetMe(session).AssertError(HttpStatusCode.Unauthorized);
        }
        
        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(LogoutData), nameof(LogoutData.NegativeLogoutData))]
        public void BaseNegativeMyUserTest(string session)
        {
            _facade.PostLogout(session).AssertError(HttpStatusCode.Unauthorized);
        }
    }
}