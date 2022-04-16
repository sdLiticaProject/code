using System.Net.Http;
using Serilog;

namespace sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions
{
    public static class HttpClientExtension
    {
        public static HttpResponseMessage LogAndPost(this HttpClient client, string url, HttpContent content,
            ILogger logger)
        {
            logger.Information("Sending POST request to " + url);
            logger.Debug("==== Request headers (start) ====");
            logger.Debug(client.DefaultRequestHeaders.ToString());
            logger.Debug(content.Headers.ToString());
            logger.Debug("==== Request headers (end) ====");
            logger.Debug("==== Request content (start) ====");
            logger.Debug(content.ReadAsStringAsync().Result);
            logger.Debug("==== Request content (end) ====");
            return client.PostAsync(url, content).Result.Log();
        }

        public static HttpResponseMessage LogAndPut(this HttpClient client, string url, HttpContent content,
            ILogger logger)
        {
            logger.Information("Sending PUT request to " + url);
            logger.Debug("==== Request headers (start) ====");
            logger.Debug(client.DefaultRequestHeaders.ToString());
            logger.Debug(content.Headers.ToString());
            logger.Debug("==== Request headers (end) ====");
            logger.Debug("==== Request content (start) ====");
            logger.Debug(content.ReadAsStringAsync().Result);
            logger.Debug("==== Request content (end) ====");
            return client.PutAsync(url, content).Result.Log();
        }
        
        public static HttpResponseMessage LogAndGet(this HttpClient client, string url,
            ILogger logger)
        {
            logger.Information("Sending GET request to " + url);
            logger.Debug("==== Request headers (start) ====");
            logger.Debug(client.DefaultRequestHeaders.ToString());
            logger.Debug("==== Request headers (end) ====");
            return client.GetAsync(url).Result.Log();
        }
        
        public static HttpResponseMessage LogAndDelete(this HttpClient client, string url,
            ILogger logger)
        {
            logger.Information("Sending DELETE request to " + url);
            logger.Debug("==== Request headers (start) ====");
            logger.Debug(client.DefaultRequestHeaders.ToString());
            logger.Debug("==== Request headers (end) ====");
            return client.DeleteAsync(url).Result.Log();
        }
    }
}
