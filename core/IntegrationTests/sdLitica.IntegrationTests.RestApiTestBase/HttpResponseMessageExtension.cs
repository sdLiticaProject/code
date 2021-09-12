using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Serilog;

namespace sdLitica.IntegrationTests.RestApiTestBase
{
    public static class HttpResponseMessageExtension
    {
        private static ILogger Logger;

        public static void Init(ILogger logger)
        {
            Logger = logger;
        }

        public static HttpResponseMessage AssertSuccess(this HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.AnyOf(
                    HttpStatusCode.OK,
                    HttpStatusCode.Created,
                    HttpStatusCode.Accepted,
                    HttpStatusCode.NonAuthoritativeInformation,
                    HttpStatusCode.NoContent,
                    HttpStatusCode.ResetContent,
                    HttpStatusCode.PartialContent,
                    HttpStatusCode.MultiStatus,
                    HttpStatusCode.AlreadyReported,
                    HttpStatusCode.IMUsed
                ),
                $"Expected to have status 2XX, but found {response.StatusCode}. Body was '{response.Content.ReadAsStringAsync().Result}'");
            return response;
        }

        public static HttpResponseMessage AssertError(this HttpResponseMessage response, HttpStatusCode exitCode)
        {
            Assert.That(response.StatusCode, Is.EqualTo(exitCode),
                $"Expected to have status {exitCode}, but found {response.StatusCode}. Body was {response.Content.ReadAsStringAsync().Result}");
            return response;
        }

        public static T MapAndLog<T>(this HttpResponseMessage response)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            Logger.Information($"Got response:\n{responseString}");
            return JObject.Parse(responseString)
                .ToObject<T>();
        }

        public static string GetTokenFromResponse(this HttpResponseMessage response)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            JToken jToken = JObject.Parse(responseString).SelectToken(CommonHttpConstants.AuthorizationTokenResponse);

            if (jToken == null)
            {
                throw new NullReferenceException($"Got no token from response '{responseString}'");
            }

            return jToken.Value<string>();
        }

        public static DateTimeOffset GetTokenExpirationDateFromResponse(this HttpResponseMessage response)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            JToken jToken = JObject.Parse(responseString)
                .SelectToken(CommonHttpConstants.AuthorizationTokenExpirationDateResponse);
            return DateTimeOffset.MinValue.AddTicks(jToken.Value<long>());
        }
    }
}