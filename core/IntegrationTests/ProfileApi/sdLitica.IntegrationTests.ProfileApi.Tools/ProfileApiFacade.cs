using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.RestApiTestBase;
using Serilog;

namespace sdLitica.IntegrationTests.ProfileApi.Tools
{
    public class ProfileApiFacade
    {
        protected readonly ILogger Logger;
        protected readonly string BaseApiRoute;
        protected readonly string UpdateNamesRoute = "update";
        protected readonly string LoginRoute = "login";
        protected readonly string LogoutRoute = "logout";
        protected readonly string MeRoute = "me";
        protected readonly string GetApiKeysRoute = "apikeys";
        protected readonly string PostApiKeysRoute = "apikeys";
        protected readonly string DeleteApiKeysRoute = "apikeys";

        public ProfileApiFacade(ILogger logger, string rootUrl)
        {
            BaseApiRoute = $"{rootUrl}/api/v1/Profile";
            Logger = logger;
        }

        public string GetTokenFromResponse(HttpResponseMessage response)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            JToken jToken = JObject.Parse(responseString).SelectToken(CommonHttpConstants.AuthorizationTokenResponse);
            Assert.NotNull(jToken, $"Got no token from response '{responseString}'");
            return jToken.Value<string>();
        }

        public HttpResponseMessage PostCreateNewProfile(string tokenValue, TestUserModel user)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));

                if (tokenValue != null)
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
                }

                return client.LogAndPost($"{BaseApiRoute}",
                    new StringContent(user.ToString(), Encoding.UTF8, CommonHttpConstants.ApplicationJsonMedia), Logger);
            }
        }

        public HttpResponseMessage PostUpdateProfileNames(string tokenValue, TestUserUpdateModel updateUser)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));
                
                if (tokenValue != null)
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
                }
                
                return client.LogAndPost($"{BaseApiRoute}/{UpdateNamesRoute}",
                    new StringContent(updateUser.ToString(), Encoding.UTF8, CommonHttpConstants.ApplicationJsonMedia), Logger);
            }
        }

        public HttpResponseMessage PostLogin(TestLoginModel login)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));
                return client.LogAndPost($"{BaseApiRoute}/{LoginRoute}",
                    new StringContent(login.ToString(), Encoding.UTF8, CommonHttpConstants.ApplicationJsonMedia), Logger);
            }
        }
        
        public HttpResponseMessage PostLogout(string tokenValue)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));
                
                if (tokenValue != null)
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
                }
                
                return client.LogAndPost($"{BaseApiRoute}/{LogoutRoute}",
                    new StringContent(String.Empty, Encoding.UTF8, CommonHttpConstants.ApplicationJsonMedia), Logger);
            }
        }
        
        public HttpResponseMessage GetMe(string tokenValue)
        {
            using (HttpClient client = HttpClientFactory.Create())
            {
                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));
                
                if (tokenValue != null)
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
                }
                
                return client.LogAndGet($"{BaseApiRoute}/{MeRoute}", Logger);
            }
        }
    }
}