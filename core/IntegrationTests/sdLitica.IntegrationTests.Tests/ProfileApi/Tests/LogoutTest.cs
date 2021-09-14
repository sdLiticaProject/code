using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.CommonTestData;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.ProfileApi.TestData;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
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
            new GivenStatement(Logger)
                .DefaultUserLoginCredentials()
                .When
                .LoginRequestIsSend()
                .LogoutRequestIsSend()
                .Then
                .SessionTokenIsPresent()
                .SessionTokenIsInvalid();
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void TestDoubleLogout()
        {
            new GivenStatement(Logger)
                .DefaultUserLoginCredentials()
                .When
                .LoginRequestIsSend()
                .LogoutRequestIsSend()
                .LogoutRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityLow))]
        [TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
        public void BaseNegativeLogoutTest(string session)
        {
            new GivenStatement(Logger)
                .UserSession(session)
                .When
                .LogoutRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }
    }
}