using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.Test.BaseApiTest;

namespace RestApiTests
{
    [TestFixture]
    public class ProfileApiTest: BaseAuthenticatedApiTest
    {
        private static string _apiKeysPath = "/apikeys";

        StringContent GivenLoginRequestWithBadCredentials()
        {
            JObject credentialsRequestJObject = new JObject
            {
                {"email", "fake-email-addess"},
                {"password", "fake-password"}
            };
            return new StringContent(credentialsRequestJObject.ToString(), Encoding.UTF8, "application/json");
        }

        StringContent GivenCreateApiKeyRequest()
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
            StringContent content = GivenLoginRequestWithBadCredentials();

            WhenPostRequestWithoutAuthSent(Configuration.RootUrl + ProfileApiPath + LoginPath, content);
            
            ThenResponseStatusIsUnauthorized();

        }



        [Test]
        public void TestUserShouldBeAbleToCreateAnApiAccessToken()
        {
            string defaultUserToken = GivenDefaultUserAccessToken();
            WhenRequestToCreateNewApiTokenSent(defaultUserToken);
            ThenResponseStatusIsCreated();
        }

        private void WhenRequestToCreateNewApiTokenSent(string token)
        {
            AuthenticationHeaderValue header = GivenAuthenticationHeaderFromToken(token);
            StringContent newApiKeyRequest = GivenCreateApiKeyRequest();

            WhenPostRequestSent(Configuration.RootUrl + ProfileApiPath + _apiKeysPath, header, newApiKeyRequest);

        }
    }
}
