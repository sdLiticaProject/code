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
	public class DeleteTimeSeriesTests : BaseAuthorizedTimeSeriesTest
	{
		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeRemoveTimeSeriesMetadataById()
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
				.RemoveTimeSeriesRequestIsSend(timeSeries.Id)
				.GetAllTimeSeriesRequestIsSend()
				.Then
				.TimeSeriesIsNotPresentInUserTimeSeries(timeSeries);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestRemoveRemovedTimeSeriesMetadataByIdSessionToken()
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
				.RemoveTimeSeriesRequestIsSend(timeSeries.Id)
				.WithCode(HttpStatusCode.OK)
				.RemoveTimeSeriesRequestIsSend(timeSeries.Id)
				.Then
				.ResponseHasCode(HttpStatusCode.NotFound);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
		public void TestRemoveTimeSeriesMetadataByIdSessionToken(string sessionToken)
		{
			Given
				.UserSession(sessionToken)
				.When
				.RemoveTimeSeriesRequestIsSend(Guid.NewGuid().ToString())
				.Then
				.ResponseHasCode(HttpStatusCode.Unauthorized);
		}
	}
}
