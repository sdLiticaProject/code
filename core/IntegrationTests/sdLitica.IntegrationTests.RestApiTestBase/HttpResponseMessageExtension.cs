using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace sdLitica.IntegrationTests.RestApiTestBase
{
    public static class HttpResponseMessageExtension
    {
        public static HttpResponseMessage AssertSuccess(this HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                $"Expected to have status 200, but found {response.StatusCode}. Body was '{response.Content.ReadAsStringAsync().Result}'");
            return response;
        }
        
        public static HttpResponseMessage AssertError(this HttpResponseMessage response, HttpStatusCode exitCode)
        {
            Assert.That(response.StatusCode, Is.EqualTo(exitCode),
                $"Expected to have status {exitCode}, but found {response.StatusCode}. Body was {response.Content.ReadAsStringAsync().Result}");
            return response;
        }
    }
}