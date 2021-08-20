using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Serilog;

namespace sdLitica.IntegrationTests.RestApiTestBase
{
    /// <summary>
    /// This class represents a base test class for all other integration test
    /// classes
    /// </summary>
    [TestFixture]
    public class BaseApiTest
    {
        protected BaseTestConfiguration Configuration;
        
        // This object contains most recent API response
        protected HttpResponseMessage LastApiResponse;
        protected String CurrentApiAccessToken;
        protected ILogger Logger;

        private string _lastApiResponseContent;
        private JObject _lastApiJson;

        // Methods to deal with tests configuration from file
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {            
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("testsettings.json", optional: true)
                .AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
                .AddEnvironmentVariables()
                .Build();
        }

        public static BaseTestConfiguration GetApplicationConfiguration(string outputPath)
        {
            var configuration = new BaseTestConfiguration();

            var iConfig = GetIConfigurationRoot(outputPath);

            iConfig
                .GetSection("Server")
                .Bind(configuration);

            return configuration;
        }

        [OneTimeSetUp]
        public void Init()
        {
            Configuration = GetApplicationConfiguration(TestContext.CurrentContext.TestDirectory);
            Assert.NotNull(Configuration.RootUrl, "Server URL for tests executions is not set");
            Assert.NotNull(Configuration.UserName, "Default user name for tests executions is not set");
            Assert.NotNull(Configuration.Password, "Default password for tests executions is not set");

            Logger = new LoggerConfiguration().CreateLogger();
        }

        // API base invocation methods
        protected void WhenGetRequestSent(string url, AuthenticationHeaderValue credentials)
        {
            Logger.Information("Sending GET request to " + url);
            LastApiResponse = null;
            _lastApiResponseContent = null;
            _lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders.Authorization = credentials;

                LastApiResponse = client.GetAsync(url).Result;
            }
        }

        protected void WhenPostRequestSent(string url, AuthenticationHeaderValue credentials, HttpContent content)
        {
            Logger.Information("Sending POST request to " + url);
            Logger.Information("==== Request content (start) ====");
            Logger.Information(content.ReadAsStringAsync().Result);
            Logger.Information("==== Request content (end) ====");
            LastApiResponse = null;
            _lastApiResponseContent = null;
            _lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders.Authorization = credentials;
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                LastApiResponse = client.PostAsync(url, content).Result;
            }
        }
       
        protected void WhenPostRequestWithoutAuthSentWithoutContentType(string url, HttpContent content) {
            WhenPostRequestWithoutAuthSentInternal(url, content, false);
        }

        protected void WhenPostRequestWithoutAuthSent(string url, HttpContent content) {
            WhenPostRequestWithoutAuthSentInternal(url, content, true);
        }

        private void WhenPostRequestWithoutAuthSentInternal(string url, HttpContent content, bool setContentType) {
            Logger.Information("Sending POST request without authentication to " + url);
            Logger.Information("==== Request content (start) ====");
            Logger.Information(content.ReadAsStringAsync().Result);
            Logger.Information("==== Request content (end) ====");
            LastApiResponse = null;
            _lastApiResponseContent = null;
            _lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                if (setContentType) {
                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                } 

                LastApiResponse = client.PostAsync(url, content).Result;
            }
        }

        protected void WhenAuthnticationRequestisSentToServer()
        {
            WhenAuthnticationRequestisSentToServer(Configuration.UserName, Configuration.Password);
        }

        protected void WhenAuthnticationRequestisSentToServer(string userName, string password)
        {
            string url = Configuration.RootUrl + "profile/login";
            JObject credentialsRequestJObject = new JObject
            {
                {"UserName", userName},
                {"Password", password}
            };
            StringContent content = new StringContent(credentialsRequestJObject.ToString(), Encoding.UTF8, "application/json");

            WhenPostRequestWithoutAuthSent(url, content);
        }

        protected void ThenAccessTokenSuccessfullyRetrieved()
        {
            ThenResponseStatusIsOk();
            ThenAccessTokenRetrieved();
        }

        protected void ThenAccessTokenRetrieved() {
            ThenResponseIsValidJson();
            JToken jToken = _lastApiJson.SelectToken("$.Entity.Token");
            Assert.NotNull(jToken);
            CurrentApiAccessToken = jToken.Value<string>();
        }

        protected void WhenPutRequestSent(string url, HttpHeaders headers, HttpContent content)
        {
            Logger.Information("Sending PUT request to " + url);
            Logger.Information("==== Request content (start) ====");
            Logger.Information(content.ToString());
            Logger.Information("==== Request content (end) ====");
            LastApiResponse = null;
            _lastApiResponseContent = null;
            _lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                client.PutAsync(url, content);
            }
        }

        protected void WhenDeleteRequestSent(string url, AuthenticationHeaderValue credentials)
        {
            Logger.Information("Sending DELETE request to " + url);
            LastApiResponse = null;
            _lastApiResponseContent = null;
            _lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders.Authorization = credentials;

                LastApiResponse = client.DeleteAsync(url).Result;
            }
        }



        // Default validations
        private Boolean CompareStatusesAndLog(HttpStatusCode received, HttpStatusCode expected)
        {
            if (received != expected)
            {
                Logger.Information(
                    "Wrong HTTP status code: expected " +
                    expected +
                    ", received " +
                    received);
                return false;
            }
            return true;
        }

        protected void ThenResponseStatusIsOk()
        {
            Assert.IsTrue(CompareStatusesAndLog(LastApiResponse.StatusCode, HttpStatusCode.OK));
        }

        protected void ThenResponseStatusIsAccepted()
        {
            String content = "";
            if (null != _lastApiResponseContent)
            {
                content = _lastApiResponseContent;
            }
            else
            {
                content = LastApiResponse.Content.ReadAsStringAsync().Result;
            }
            Logger.Information("Content: '" + content + "'");
            Assert.IsTrue(CompareStatusesAndLog(LastApiResponse.StatusCode, HttpStatusCode.Accepted));
        }

        protected void ThenResponseStatusIsCreated()
        {
            String content = "";
            if (null != _lastApiResponseContent)
            {
                content = _lastApiResponseContent;
            }
            else
            {
                content = LastApiResponse.Content.ReadAsStringAsync().Result;
            }
            Logger.Information("Content: '" + content + "'");
            Assert.IsTrue(CompareStatusesAndLog(LastApiResponse.StatusCode, HttpStatusCode.Created));
        }

        protected void ThenResponseStatusIsBadRequest()
        {
            Assert.IsTrue(CompareStatusesAndLog(LastApiResponse.StatusCode, HttpStatusCode.BadRequest));
        }
        
        protected void ThenResponseStatusIsConflict()
        {
            Assert.IsTrue(CompareStatusesAndLog(LastApiResponse.StatusCode, HttpStatusCode.Conflict));
        }

        protected void ThenResponseStatusIsNotFound()
        {
            Assert.True(CompareStatusesAndLog(LastApiResponse.StatusCode, HttpStatusCode.NotFound));
        }

        protected void ThenResponseStatusIsUnauthorized()
        {
            Assert.True(CompareStatusesAndLog(LastApiResponse.StatusCode, HttpStatusCode.Unauthorized));
        }

        protected void ThenResponseStatusIsNoContent()
        {
            Assert.True(CompareStatusesAndLog(LastApiResponse.StatusCode, HttpStatusCode.NoContent));
        }

        protected void ThenResponseStatusIsUnsupportedMediaType()
        {
            Assert.True(CompareStatusesAndLog(LastApiResponse.StatusCode, HttpStatusCode.UnsupportedMediaType));
        }
        
        protected void ThenResponseStatusIsUnprocessable()
        {
            Assert.IsTrue(CompareStatusesAndLog(LastApiResponse.StatusCode, (HttpStatusCode)422));
        }

        protected void ThenResponseIsValidJson()
        {
            if (_lastApiJson == null)
            {
                try
                {
                    _lastApiResponseContent = LastApiResponse.Content.ReadAsStringAsync().Result;
                    _lastApiJson = JObject.Parse(_lastApiResponseContent);
                }
                catch (Exception e)
                {
                    Assert.Fail("Invalid JSON response from server. " + e.Message);
                }
            }
        }

        protected string ThenResponseContainsField(string fieldName)
        {
            Logger.Information(_lastApiResponseContent);
            ThenResponseIsValidJson();

            JToken jToken = _lastApiJson.SelectToken("$." + fieldName);
            Assert.IsNotNull(jToken);

            return jToken.ToString();
        }

        protected void ThenResponseContainsFieldWithValue(string fieldName, string expectedValue)
        {
            Logger.Information(_lastApiResponseContent);
            ThenResponseContainsField(fieldName);

            JToken jToken = _lastApiJson.SelectToken("$." + fieldName);
            Assert.AreEqual(expectedValue, jToken.Value<string>());
        }

        protected void ThenResponseContainsArrayItemWithValue(string arrayPath, string filedName, string expectedValue)
        {
            Logger.Information(
                    string.Format(
                            "Looking for object with field name '{0}' in array '{1}' with value '{2}'", 
                            filedName, 
                            arrayPath, 
                            expectedValue)
            );
            Logger.Information(_lastApiResponseContent);
            ThenResponseContainsField(arrayPath);
            
            JArray jArray = _lastApiJson.SelectToken("$." + arrayPath) as JArray;
            Logger.Information("Got array with size " + jArray.Count.ToString());

            bool itemFound = false;
            for (int i = 0; i < jArray.Count; i++)
            {
                JToken property = jArray[i].SelectToken("." + filedName);
                Logger.Information("Fount property '" + filedName + "' with value '" + property.Value<string>() + "'");
                if (property.Value<string>().Equals(expectedValue))
                {
                    itemFound = true;
                    break;
                }
            }
            Assert.True(itemFound);
        }

        protected void ThenResponseContainsArrayOfSize(string arrayName, int expectedSize)
        {
            Logger.Information(_lastApiResponseContent);
            ThenResponseContainsField(arrayName);

            JArray jArray = _lastApiJson.SelectToken("$." + arrayName) as JArray;
            Assert.AreEqual(expectedSize, jArray.Count);
        }

        protected string GetFieldValue(string fieldName)
        {
            Logger.Information(_lastApiResponseContent);
            ThenResponseIsValidJson();

            JToken jToken = _lastApiJson.SelectToken("$." + fieldName);

            return jToken.Value<string>();
        }

        protected void ThenResponseContainsArrayOfSizeGreaterOrEqual(string arrayName, int expectedSize)
        {
            Logger.Information(_lastApiResponseContent);
            ThenResponseContainsField(arrayName);

            JArray jArray = _lastApiJson.SelectToken("$." + arrayName) as JArray;
            Assert.GreaterOrEqual(jArray.Count, expectedSize);
        }

        protected AuthenticationHeaderValue GivenAuthenticationHeaderFromToken(String tokenValue)
        {
            return new AuthenticationHeaderValue("cloudToken", tokenValue);
        }

        protected AuthenticationHeaderValue GivenExistingUserCredentials()
        {
            WhenAuthnticationRequestisSentToServer();
            ThenAccessTokenSuccessfullyRetrieved();
            return new AuthenticationHeaderValue("cloudToken", CurrentApiAccessToken);
        }

        protected AuthenticationHeaderValue GivenNotExistingUserCredentials()
        {
            var byteArrayCredentials = Encoding.ASCII.GetBytes(Configuration.UserName + "lalala" + ":" + Configuration.Password + "dsds");
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArrayCredentials));
        }
    }
}