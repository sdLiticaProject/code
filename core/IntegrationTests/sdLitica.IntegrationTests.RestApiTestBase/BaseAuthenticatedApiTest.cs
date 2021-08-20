using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace sdLitica.IntegrationTests.RestApiTestBase
{
    /// <summary>
    /// This class represents a base test class for all other integration test
    /// classes
    /// </summary>
    [TestFixture]
    public class BaseAuthenticatedApiTest : BaseApiTest
    {
        // Base path to Profile API's
        protected static readonly string ProfileApiPath = "/api/v1/Profile";

        // Endpoint for login requests
        protected static readonly string LoginPath = "/login";

        [OneTimeSetUp]
        public void InitAuth()
        {
            ensureDefaultUserIsPresentOnServer();
        }

        private void ensureDefaultUserIsPresentOnServer()
        {
            StringContent registrationPayload =
                GivenRequestToRegisterEndUser(Configuration.UserName, Configuration.Password);

            WhenUserRegistrationRequestIsSent(registrationPayload);

            ThenTestUserSuccessfullySeededToTheServer();
        }

        protected StringContent GivenRequestToRegisterEndUser(string userEmail, string password)
        {
            JObject credentialsRequestJObject = new JObject
            {
                {"email", userEmail},
                {"password", password}
            };
            return new StringContent(credentialsRequestJObject.ToString(), Encoding.UTF8, "application/json");
        }

        protected StringContent GivenLoginRequestWithDefaultCredentials()
        {
            JObject credentialsRequestJObject = new JObject
            {
                {"email", Configuration.UserName},
                {"password", Configuration.Password}
            };
            return new StringContent(credentialsRequestJObject.ToString(), Encoding.UTF8, "application/json");
        }

        protected string GivenDefaultUserAccessToken()
        {
            string result = "";

            StringContent defaultUserLoginRequest =
                GivenLoginRequestWithDefaultCredentials();

            WhenAccessTokenIsRequested(defaultUserLoginRequest);

            result = ThenAccessTokenSuccessfullyReceived();

            return result;
        }

        void WhenUserRegistrationRequestIsSent(StringContent requestContent)
        {
            WhenPostRequestWithoutAuthSent(Configuration.RootUrl + ProfileApiPath, requestContent);
        }

        void WhenAccessTokenIsRequested(StringContent requestContent)
        {
            WhenPostRequestWithoutAuthSent(Configuration.RootUrl + ProfileApiPath + LoginPath, requestContent);
        }

        string ThenAccessTokenSuccessfullyReceived()
        {
            string result = "";

            ThenResponseStatusIsOk();
            result = ThenResponseContainsField("entity.token");

            return result;
        }

        void ThenTestUserSuccessfullySeededToTheServer()
        {
            if (LastApiResponse.StatusCode.Equals(HttpStatusCode.Created))
            {
                Console.WriteLine("[Setup]: Test user successfully created on the server");
            }
            else if (LastApiResponse.StatusCode.Equals(HttpStatusCode.Conflict))
            {
                Console.WriteLine("[Setup]: Test user already exists on the server");
            }
            else
            {
                Assert.Fail($"[Setup]: Unable to seed default user to the test server. Response code :'{LastApiResponse.StatusCode}'");
            }
        }
    }
}