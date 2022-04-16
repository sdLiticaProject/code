using System.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Tests
{
	public class AppendTimeSeriesDataTests : BaseAuthorizedTimeSeriesTest
	{
		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeAppendTimeSeriesData()
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.UploadTimeSeriesDataRequestIsSend("timestamp,value\n2021-01-10 11:14,1\n2021-01-10 11:16,3")
				.WithSuccess()
				.AppendTimeSeriesDataRequestIsSend(new JArray(
					new JObject(
						new JProperty("timestamp", "2021-01-10 12:24"),
						new JProperty("value", "6")
					), new JObject(
						new JProperty("timestamp", "2021-01-10 15:26"),
						new JProperty("value", "-20")
					)))
				.WithSuccess()
				.GetTimeSeriesDataRequestIsSend()
				.WithSuccess()
				.Then
				.TimeSeriesDataEntitiesCountIsEqualTo(4);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void AppendTimeSeriesDataRowsWithoutTimestampProp()
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.UploadTimeSeriesDataRequestIsSend("timestamp,value\n2021-01-10 11:14,1\n2021-01-10 11:16,3")
				.WithSuccess()
				.AppendTimeSeriesDataRequestIsSend(new JArray(
					new JObject(
						new JProperty("timestamp", "2021-01-10 12:24"),
						new JProperty("value", "6")
					), new JObject(
						new JProperty("time", "2021-01-10 15:26"),
						new JProperty("value", "-20")
					)))
				.WithCode(HttpStatusCode.BadRequest)
				.GetTimeSeriesDataRequestIsSend()
				.WithSuccess()
				.Then
				.TimeSeriesDataEntitiesCountIsEqualTo(2);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void AppendTimeSeriesDataRowsWithoutValueProp()
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.UploadTimeSeriesDataRequestIsSend("timestamp,value\n2021-01-10 11:14,1\n2021-01-10 11:16,3")
				.WithSuccess()
				.AppendTimeSeriesDataRequestIsSend(new JArray(
					new JObject(
						new JProperty("timestamp", "2021-01-10 12:24"),
						new JProperty("wrong_value", "6")
					), new JObject(
						new JProperty("timestamp", "2021-01-10 15:26"),
						new JProperty("value", "-20")
					)))
				.WithCode(HttpStatusCode.BadRequest)
				.GetTimeSeriesDataRequestIsSend()
				.WithSuccess()
				.Then
				.TimeSeriesDataEntitiesCountIsEqualTo(2);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void AppendTimeSeriesDataRowsWithNewValueProp()
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.UploadTimeSeriesDataRequestIsSend("timestamp,value\n2021-01-10 11:14,1\n2021-01-10 11:16,3")
				.WithSuccess()
				.AppendTimeSeriesDataRequestIsSend(new JArray(
					new JObject(
						new JProperty("timestamp", "2021-01-10 12:24"),
						new JProperty("wrong_value", "6"),
						new JProperty("value", "6")
					), new JObject(
						new JProperty("timestamp", "2021-01-10 15:26"),
						new JProperty("value", "-20")
					)))
				.WithCode(HttpStatusCode.BadRequest)
				.GetTimeSeriesDataRequestIsSend()
				.WithSuccess()
				.Then
				.TimeSeriesDataEntitiesCountIsEqualTo(2);
		}
	}
}
