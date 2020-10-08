using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace sdLitica.Test.BaseApiTest
{
    /// <summary>
    /// This class represents a base test class for all other integration test
    /// classes
    /// </summary>
    [TestFixture]
    public class BaseApiTest
    {
        protected BaseTestConfiguration configuration;
        
        // This object contains most recent API response
        private HttpResponseMessage lastApiResponse;
        protected String currentApiAccessToken;

        private string lastApiResponseContent;
        private JObject lastApiJson;

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

        [SetUp]
        public void Init()
        {
            configuration = GetApplicationConfiguration(TestContext.CurrentContext.TestDirectory);
        }


        // API base invocation methods
        protected void whenGetRequestSent(string url, AuthenticationHeaderValue credentials)
        {
            Console.WriteLine("Sending GET request to " + url);
            lastApiResponse = null;
            lastApiResponseContent = null;
            lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders.Authorization = credentials;

                lastApiResponse = client.GetAsync(url).Result;
            }
        }

        protected void whenPostRequestSent(string url, AuthenticationHeaderValue credentials, HttpContent content)
        {
            Console.WriteLine("Sending POST request to " + url);
            Console.WriteLine("==== Request content (start) ====");
            Console.WriteLine(content.ReadAsStringAsync().Result);
            Console.WriteLine("==== Request content (end) ====");
            lastApiResponse = null;
            lastApiResponseContent = null;
            lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders.Authorization = credentials;
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                lastApiResponse = client.PostAsync(url, content).Result;
            }
        }
       
        protected void whenPostRequestWithoutAuthSentWithoutContentType(string url, HttpContent content) {
            whenPostRequestWithoutAuthSentInternal(url, content, false);
        }

        protected void whenPostRequestWithoutAuthSent(string url, HttpContent content) {
            whenPostRequestWithoutAuthSentInternal(url, content, true);
        }

        private void whenPostRequestWithoutAuthSentInternal(string url, HttpContent content, bool setContentType) {
            Console.WriteLine("Sending POST request without authentication to " + url);
            Console.WriteLine("==== Request content (start) ====");
            Console.WriteLine(content.ReadAsStringAsync().Result);
            Console.WriteLine("==== Request content (end) ====");
            lastApiResponse = null;
            lastApiResponseContent = null;
            lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                if (setContentType) {
                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                } 

                lastApiResponse = client.PostAsync(url, content).Result;
            }
        }

        protected void whenAuthnticationRequestisSentToServer()
        {
            whenAuthnticationRequestisSentToServer(configuration.UserName, configuration.Password);
        }

        protected void whenAuthnticationRequestisSentToServer(string userName, string password)
        {
            string url = configuration.RootUrl + "profile/login";
            JObject credentialsRequestJObject = new JObject
            {
                {"UserName", userName},
                {"Password", password}
            };
            StringContent content = new StringContent(credentialsRequestJObject.ToString(), Encoding.UTF8, "application/json");

            whenPostRequestWithoutAuthSent(url, content);
        }

        protected void thenAccessTokenSuccessfullyRetrieved()
        {
            thenResponseStatusIsOK();
            thenAccessTokenRetrieved();
        }

        protected void thenAccessTokenRetrieved() {
            thenResponseIsValidJson();
            JToken jToken = lastApiJson.SelectToken("$.Entity.Token");
            currentApiAccessToken = jToken.Value<string>();
        }

        protected void whenPutRequestSent(string url, HttpHeaders headers, HttpContent content)
        {
            Console.WriteLine("Sending PUT request to " + url);
            Console.WriteLine("==== Request content (start) ====");
            Console.WriteLine(content.ToString());
            Console.WriteLine("==== Request content (end) ====");
            lastApiResponse = null;
            lastApiResponseContent = null;
            lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                client.PutAsync(url, content);
            }
        }

        protected void whenDeleteRequestSent(string url, AuthenticationHeaderValue credentials)
        {
            Console.WriteLine("Sending DELETE request to " + url);
            lastApiResponse = null;
            lastApiResponseContent = null;
            lastApiJson = null;

            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders.Authorization = credentials;

                lastApiResponse = client.DeleteAsync(url).Result;
            }
        }



        // Default validations
        private Boolean CompareStatusesAndLog(HttpStatusCode received, HttpStatusCode expected)
        {
            if (received != expected)
            {
                Console.WriteLine(
                    "Wrong HTTP status code: expected " +
                    expected +
                    ", received " +
                    received);
                return false;
            }
            return true;
        }

        protected void thenResponseStatusIsOK()
        {
            Assert.IsTrue(CompareStatusesAndLog(lastApiResponse.StatusCode, HttpStatusCode.OK));
        }

        protected void thenResponseStatusIsAccepted()
        {
            String content = "";
            if (null != lastApiResponseContent)
            {
                content = lastApiResponseContent;
            }
            else
            {
                content = lastApiResponse.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine("Content: '" + content + "'");
            Assert.IsTrue(CompareStatusesAndLog(lastApiResponse.StatusCode, HttpStatusCode.Accepted));
        }

        protected void thenResponseStatusIsCreated()
        {
            String content = "";
            if (null != lastApiResponseContent)
            {
                content = lastApiResponseContent;
            }
            else
            {
                content = lastApiResponse.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine("Content: '" + content + "'");
            Assert.IsTrue(CompareStatusesAndLog(lastApiResponse.StatusCode, HttpStatusCode.Created));
        }

        protected void thenResponseStatusIsBadRequest()
        {
            Assert.IsTrue(CompareStatusesAndLog(lastApiResponse.StatusCode, HttpStatusCode.BadRequest));
        }
        
        protected void thenResponseStatusIsConflict()
        {
            Assert.IsTrue(CompareStatusesAndLog(lastApiResponse.StatusCode, HttpStatusCode.Conflict));
        }

        protected void thenResponseStatusIsNotFound()
        {
            Assert.True(CompareStatusesAndLog(lastApiResponse.StatusCode, HttpStatusCode.NotFound));
        }

        protected void thenResponseStatusIsUnauthorized()
        {
            Assert.True(CompareStatusesAndLog(lastApiResponse.StatusCode, HttpStatusCode.Unauthorized));
        }

        protected void thenResponseStatusIsNoContent()
        {
            Assert.True(CompareStatusesAndLog(lastApiResponse.StatusCode, HttpStatusCode.NoContent));
        }

        protected void thenResponseStatusIsUnsupportedMediaType()
        {
            Assert.True(CompareStatusesAndLog(lastApiResponse.StatusCode, HttpStatusCode.UnsupportedMediaType));
        }
        
        protected void thenResponseStatusIsUnprocessable()
        {
            Assert.IsTrue(CompareStatusesAndLog(lastApiResponse.StatusCode, (HttpStatusCode)422));
        }

        protected void thenResponseIsValidJson()
        {
            if (lastApiJson == null)
            {
                try
                {
                    lastApiResponseContent = lastApiResponse.Content.ReadAsStringAsync().Result;
                    lastApiJson = JObject.Parse(lastApiResponseContent);
                }
                catch (Exception e)
                {
                    Assert.Fail("Invalid JSON response from server. " + e.Message);
                }
            }
        }

        protected void thenResponseContainsField(string fieldName)
        {
            Console.WriteLine(lastApiResponseContent);
            thenResponseIsValidJson();

            JToken jToken = lastApiJson.SelectToken("$." + fieldName);
            Assert.IsNotNull(jToken);
        }

        protected void thenResponseContainsFieldWithValue(string fieldName, string expectedValue)
        {
            Console.WriteLine(lastApiResponseContent);
            thenResponseContainsField(fieldName);

            JToken jToken = lastApiJson.SelectToken("$." + fieldName);
            Assert.AreEqual(expectedValue, jToken.Value<string>());
        }

        protected void thenResponseContainsArrayItemWithValue(string arrayPath, string filedName, string expectedValue)
        {
            Console.WriteLine(
                    string.Format(
                            "Looking for object with field name '{0}' in array '{1}' with value '{2}'", 
                            filedName, 
                            arrayPath, 
                            expectedValue)
            );
            Console.WriteLine(lastApiResponseContent);
            thenResponseContainsField(arrayPath);
            
            JArray jArray = lastApiJson.SelectToken("$." + arrayPath) as JArray;
            Console.WriteLine("Got array with size " + jArray.Count.ToString());

            bool itemFound = false;
            for (int i = 0; i < jArray.Count; i++)
            {
                JToken property = jArray[i].SelectToken("." + filedName);
                Console.WriteLine("Fount property '" + filedName + "' with value '" + property.Value<string>() + "'");
                if (property.Value<string>().Equals(expectedValue))
                {
                    itemFound = true;
                    break;
                }
            }
            Assert.True(itemFound);
        }

        protected void thenResponseContainsArrayOfSize(string arrayName, int expectedSize)
        {
            Console.WriteLine(lastApiResponseContent);
            thenResponseContainsField(arrayName);

            JArray jArray = lastApiJson.SelectToken("$." + arrayName) as JArray;
            Assert.AreEqual(expectedSize, jArray.Count);
        }

        protected string getFieldValue(string fieldName)
        {
            Console.WriteLine(lastApiResponseContent);
            thenResponseIsValidJson();

            JToken jToken = lastApiJson.SelectToken("$." + fieldName);

            return jToken.Value<string>();
        }

        protected void thenResponseContainsArrayOfSizeGreaterOrEqual(string arrayName, int expectedSize)
        {
            Console.WriteLine(lastApiResponseContent);
            thenResponseContainsField(arrayName);

            JArray jArray = lastApiJson.SelectToken("$." + arrayName) as JArray;
            Assert.GreaterOrEqual(jArray.Count, expectedSize);
        }

        protected AuthenticationHeaderValue givenExistingUserCredentials()
        {
            whenAuthnticationRequestisSentToServer();
            thenAccessTokenSuccessfullyRetrieved();
            return new AuthenticationHeaderValue("cloudToken", currentApiAccessToken);
        }

        protected AuthenticationHeaderValue givenNotExistingUserCredentials()
        {
            var byteArrayCredentials = Encoding.ASCII.GetBytes(configuration.UserName + "lalala" + ":" + configuration.Password + "dsds");
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArrayCredentials));
        }
    }
}