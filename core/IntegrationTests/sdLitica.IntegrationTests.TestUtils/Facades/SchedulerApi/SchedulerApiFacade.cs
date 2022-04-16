using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;
using Serilog;

namespace sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi
{
	public class SchedulerApiFacade
	{
		protected readonly ILogger Logger;
		protected readonly string BaseApiRoute;

		public SchedulerApiFacade(ILogger logger, string rootUrl)
		{
			BaseApiRoute = $"{rootUrl}/api/v1/triggers";
			Logger = logger;
		}

		public HttpResponseMessage PostCreateTrigger(string tokenValue, TestCreateNewTriggerModel model)
		{
			using HttpClient client = HttpClientFactory.Create();
			client.DefaultRequestHeaders
				.Accept
				.Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));

			if (tokenValue != null)
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
			}

			return client.LogAndPost($"{BaseApiRoute}",
				new StringContent(model.ToString(), Encoding.UTF8, CommonHttpConstants.ApplicationJsonMedia),
				Logger);
		}

		public HttpResponseMessage PutEditTrigger(string tokenValue, string metadataId, TestEditTriggerModel model)
		{
			using HttpClient client = HttpClientFactory.Create();
			client.DefaultRequestHeaders
				.Accept
				.Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));

			if (tokenValue != null)
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
			}

			return client.LogAndPut($"{BaseApiRoute}/{metadataId}",
				new StringContent(model.ToString(), Encoding.UTF8, CommonHttpConstants.ApplicationJsonMedia),
				Logger);
		}

		public HttpResponseMessage DeleteRemoveTrigger(string tokenValue, string metadataId)
		{
			using HttpClient client = HttpClientFactory.Create();
			client.DefaultRequestHeaders
				.Accept
				.Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));

			if (tokenValue != null)
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
			}

			return client.LogAndDelete($"{BaseApiRoute}/{metadataId}",
				Logger);
		}

		public HttpResponseMessage GetAllTriggers(string tokenValue)
		{
			using HttpClient client = HttpClientFactory.Create();
			client.DefaultRequestHeaders
				.Accept
				.Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));

			if (tokenValue != null)
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
			}

			return client.LogAndGet($"{BaseApiRoute}", Logger);
		}

		public HttpResponseMessage PostPauseTrigger(string tokenValue, string metadataId)
		{
			using HttpClient client = HttpClientFactory.Create();
			client.DefaultRequestHeaders
				.Accept
				.Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));

			if (tokenValue != null)
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
			}

			return client.LogAndPost($"{BaseApiRoute}/{metadataId}/pause", new StringContent(string.Empty), Logger);
		}

		public HttpResponseMessage PostResumeTrigger(string tokenValue, string metadataId)
		{
			using HttpClient client = HttpClientFactory.Create();
			client.DefaultRequestHeaders
				.Accept
				.Add(new MediaTypeWithQualityHeaderValue(CommonHttpConstants.ApplicationJsonMedia));

			if (tokenValue != null)
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue(CommonHttpConstants.AuthorizationHeader, tokenValue);
			}

			return client.LogAndPost($"{BaseApiRoute}/{metadataId}/resume", new StringContent(string.Empty), Logger);
		}
	}
}
