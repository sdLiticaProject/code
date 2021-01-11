using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.Test.BaseApiTest;
namespace sdLitica.Test.RestApiTests
{
    [TestFixture]
    public class ProfileApiTest: BaseApiTest.BaseApiTest
    {
        // Base path to Profile API's
        private static string PROFILE_API_PATH = "/api/v1/Profile";
        // Endpoint for login requests
        private static string LOGIN_PATH = "/login";

        StringContent givenLoginRequestWithBadCredentials()
        {
            JObject credentialsRequestJObject = new JObject
            {
                {"email", "fake-email-addess"},
                {"password", "fake-password"}
            };
            return new StringContent(credentialsRequestJObject.ToString(), Encoding.UTF8, "application/json");
        }

        [Test]
        public void TestServiceShouldReturnUnauthorizedWhenLoginWithWrongCredentials()
        {
            StringContent content = givenLoginRequestWithBadCredentials();

            whenPostRequestWithoutAuthSent(configuration.RootUrl + PROFILE_API_PATH + LOGIN_PATH, content);
            
            thenResponseStatusIsUnauthorized();

        }
    }
}
