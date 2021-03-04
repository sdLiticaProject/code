using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace sdLitica.Test.BaseApiTest
{
    /// <summary>
    /// This class represents a base test class for all other integration test
    /// classes
    /// </summary>
    [TestFixture]
    public class BaseAuthenticatedApiTest: BaseApiTest
    {
        // Base path to Profile API's
        private static string PROFILE_API_PATH = "/api/v1/Profile";
        // Endpoint for login requests
        private static string LOGIN_PATH = "/login";
        
        [SetUp]
        public void InitAuth()
        {
            ensureDefaultUserIsPresentOnServer();
        }

        private void ensureDefaultUserIsPresentOnServer()
        {
            StringContent registrationPayload = 
                givenRequestToRegisterEndUser(configuration.UserName, configuration.Password);

            whenUserRegistrationRequestIsSent(registrationPayload);

            thenTestUserSuccessfullySeededToTheServer();
        }

        protected StringContent givenRequestToRegisterEndUser(string userEmail, string password)
        {
            JObject credentialsRequestJObject = new JObject
            {
                {"email", userEmail},
                {"password", password}
            };
            return new StringContent(credentialsRequestJObject.ToString(), Encoding.UTF8, "application/json");
        }

        protected StringContent givenLoginRequestWithDefaultCredentials()
        {
            JObject credentialsRequestJObject = new JObject
            {
                {"email", configuration.UserName},
                {"password", configuration.Password}
            };
            return new StringContent(credentialsRequestJObject.ToString(), Encoding.UTF8, "application/json");
        }

        protected string givenDefaultUserAccessToken()
        {
            string result = "";

            StringContent defaultUserLoginRequest = 
                givenLoginRequestWithDefaultCredentials();

            whenAccessTokenIsRequested(defaultUserLoginRequest);

            result = thenAccessTokenSuccessfullyReceived();

            return result;
        }

        void whenUserRegistrationRequestIsSent(StringContent requestContent)
        {
            whenPostRequestWithoutAuthSent(configuration.RootUrl + PROFILE_API_PATH, requestContent);
        }

        void whenAccessTokenIsRequested(StringContent requestContent)
        {
            whenPostRequestWithoutAuthSent(configuration.RootUrl + PROFILE_API_PATH + LOGIN_PATH, requestContent);
        }

        string thenAccessTokenSuccessfullyReceived()
        {
            string result = "";

            thenResponseStatusIsOK();
            result = thenResponseContainsField("entity.token");

            return result;
        }

        void thenTestUserSuccessfullySeededToTheServer()
        {
            if (lastApiResponse.StatusCode.Equals(HttpStatusCode.Created))
            {
                Console.WriteLine("[Setup]: Test user successfully created on the server");
            }
            else if (lastApiResponse.StatusCode.Equals(HttpStatusCode.Conflict))
            {
                Console.WriteLine("[Setup]: Test user already exists on the server");
            }
            else
            {
                Assert.Fail("[Setup]: Unable to seed default user to the test server");
            }
        }
    }
}