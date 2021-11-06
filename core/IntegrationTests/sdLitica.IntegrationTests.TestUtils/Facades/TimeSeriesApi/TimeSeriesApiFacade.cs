using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;
using Serilog;

namespace sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi
{
	public class TimeSeriesApiFacade
	{
		protected readonly ILogger Logger;
		protected readonly string BaseApiRoute;
		protected readonly string DataApiSuffix = "data";
		protected readonly string AllDataApiSuffix = "data/all";

		public TimeSeriesApiFacade(ILogger logger, string rootUrl)
		{
			BaseApiRoute = $"{rootUrl}/api/v1/timeseries";
			Logger = logger;
		}

		public HttpResponseMessage PostCreateTimeSeries(string tokenValue, TestTimeSeriesMetadataModel model)
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
					new StringContent(model.ToString(), Encoding.UTF8, CommonHttpConstants.ApplicationJsonMedia),
					Logger);
			}
		}

		public HttpResponseMessage PostUploadTimeSeries(string tokenValue, string metadataId, string fileContent)
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

				var form =
					new MultipartFormDataContent(DateTimeOffset.UtcNow.ToString());
				form.Add(new StringContent(fileContent), "formFile", "testFile");

				return client.LogAndPost($"{BaseApiRoute}/{metadataId}/{DataApiSuffix}",
					form,
					Logger);
			}
		}

		public HttpResponseMessage GetAllTimeSeries(string tokenValue)
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

				return client.LogAndGet($"{BaseApiRoute}", Logger);
			}
		}

		public HttpResponseMessage PostUpdateTimeSeries(string tokenValue, string metadataId,
			TestTimeSeriesMetadataModel model)
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

				return client.LogAndPost($"{BaseApiRoute}/{metadataId}",
					new StringContent(model.ToString(), Encoding.UTF8, CommonHttpConstants.ApplicationJsonMedia),
					Logger);
			}
		}

		public HttpResponseMessage GetTimeSeriesMetadataById(string tokenValue, string metadataId)
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

				return client.LogAndGet($"{BaseApiRoute}/{metadataId}",
					Logger);
			}
		}

		public HttpResponseMessage GetAllTimeSeriesDataById(string tokenValue, string metadataId)
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

				return client.LogAndGet($"{BaseApiRoute}/{metadataId}/{AllDataApiSuffix}",
					Logger);
			}
		}

		public HttpResponseMessage GetTimeSeriesDataById(string tokenValue, string metadataId, string from, string to,
			string step, string pageSize, string offset)
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

				var query = HttpUtility.ParseQueryString(
					new UriBuilder($"{BaseApiRoute}/{metadataId}/{DataApiSuffix}").ToString());

				query.Add("from", from);
				query.Add("to", to);
				query.Add("step", step);
				query.Add("pageSize", pageSize);
				query.Add("offset", offset);

				return client.LogAndGet(query.ToString(),
					Logger);
			}
		}

		public HttpResponseMessage DeleteTimeSeriesById(string tokenValue, string metadataId)
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

				return client.LogAndDelete($"{BaseApiRoute}/{metadataId}",
					Logger);
			}
		}
	}
}
