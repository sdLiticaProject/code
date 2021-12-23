using System;
using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.CommonTestData;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Tests
{
	public class GetTimeSeriesMetadataByIdTests : BaseAuthorizedTimeSeriesTest
	{
		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeGetTimeSeriesMetadataById()
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
				.GetTimeSeriesMetadataByIdRequestIsSend(timeSeries.Id)
				.Then
				.TimeSeriesByIdIsEqualTo(timeSeries);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeGetAfterUpdateTimeSeriesMetadata()
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
				.UpdateGivenUpdateTimeSeriesIdsFromModel(timeSeries)
				.UpdateTimeSeriesRequestIsSend()
				.GetTimeSeriesMetadataByIdRequestIsSend(timeSeries.Id)
				.Then
				.TimeSeriesByIdIsEqualTo(updatedTimeSeries);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
		public void TestGetTimeSeriesMetadataByIdSessionToken(string sessionToken)
		{
			Given
				.UserSession(sessionToken)
				.When
				.GetTimeSeriesMetadataByIdRequestIsSend(Guid.NewGuid().ToString())
				.Then
				.ResponseHasCode(HttpStatusCode.Unauthorized);
		}
	}
}
