using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.TestData;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Tests
{
    public class LogoutTest : ProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestSmokeLogout()
        {
            var session = Facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess().GetTokenFromResponse();
            Facade.PostLogout(session).AssertSuccess();

            Facade.GetMe(session).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestDoubleLogout()
        {
            var session = Facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess().GetTokenFromResponse();
            Facade.PostLogout(session).AssertSuccess();
            Facade.PostLogout(session).AssertError(HttpStatusCode.Unauthorized);

            Facade.GetMe(session).AssertError(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(LogoutData), nameof(LogoutData.NegativeLogoutData))]
        public void BaseNegativeMyUserTest(string session)
        {
            Facade.PostLogout(session).AssertError(HttpStatusCode.Unauthorized);
        }
    }
}