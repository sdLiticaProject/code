using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tools;
using sdLitica.IntegrationTests.RestApiTestBase;
using Serilog;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    [TestFixture]
    public class ProfileApiTest : BaseAuthenticatedApiTest
    {
        private static string _apiKeysPath = "/apikeys";
        protected ProfileApiFacade _facade;

        [OneTimeSetUp]
        public void InitFacade()
        {
            // correct config
            Assert.AreEqual("user@sdcliud.io", Configuration.UserName);
            // todo bootstrap it later
            _facade = new ProfileApiFacade(
                new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger(),
                Configuration.RootUrl);
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