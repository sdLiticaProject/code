using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.Test.BaseApiTest;
namespace sdLitica.Test.RestApiTests
{
    [TestFixture]
    public class ProfileApiTest: BaseAuthenticatedApiTest
    {
        // Base path to Profile API's
        private static string PROFILE_API_PATH = "/api/v1/Profile";
        // Endpoint for login requests
        private static string LOGIN_PATH = "/login";

        private static string API_KEYS_PATH = "/apikeys";

        private static int seed = (new Random()).Next();

        StringContent givenLoginRequestWithBadCredentials()
        {
            JObject credentialsRequestJObject = new JObject
            {
                {"email", "fake-email-addess"},
                {"password", "fake-password"}
            };
            return new StringContent(credentialsRequestJObject.ToString(), Encoding.UTF8, "application/json");
        }

        StringContent givenCreateApiKeyRequest()
        {
            JObject credentialsRequestJObject = new JObject
            {
                {"description", "Some description for API key"}
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



        [Test]
        public void TestUserShouldBeAbleToCreateAnAPIAccessToken()
        {
            string defaultUserToken = givenDefaultUserAccessToken();
            whenRequestToCreateNewApiTokenSent(defaultUserToken);
            thenResponseStatusIsCreated();
        }

        private void whenRequestToCreateNewApiTokenSent(string token)
        {
            AuthenticationHeaderValue header = givenAuthenticationHeaderFromToken(token);
            StringContent newApiKeyRquest = givenCreateApiKeyRequest();

            whenPostRequestSent(configuration.RootUrl + PROFILE_API_PATH + API_KEYS_PATH, header, newApiKeyRquest);

        }
    }
}
