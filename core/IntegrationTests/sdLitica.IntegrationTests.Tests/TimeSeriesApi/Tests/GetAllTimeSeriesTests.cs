using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.CommonTestData;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Tests
{
	public class GetAllTimeSeriesTests : BaseAuthorizedTimeSeriesTest
	{
		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeGetAllTimeSeries()
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
				.GetAllTimeSeriesRequestIsSend()
				.Then
				.TimeSeriesIsPresentInUserTimeSeries(timeSeries);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeGetAllAfterUpdateTimeSeries()
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};

			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};

			Given
				.NewTimeSeries(timeSeries)
				.UpdateTimeSeries(updatedTimeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.UpdateTimeSeriesRequestIsSend()
				.GetAllTimeSeriesRequestIsSend()
				.Then
				.TimeSeriesIsPresentInUserTimeSeries(updatedTimeSeries);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
		public void TestGetAllTimeSeriesSessionToken(string sessionToken)
		{
			Given
				.UserSession(sessionToken)
				.When
				.GetAllTimeSeriesRequestIsSend()
				.Then
				.ResponseHasCode(HttpStatusCode.Unauthorized);
		}
	}
}
