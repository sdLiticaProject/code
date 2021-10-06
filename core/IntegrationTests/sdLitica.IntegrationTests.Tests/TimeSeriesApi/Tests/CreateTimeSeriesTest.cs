using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.CommonTestData;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.TestData;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Tests
{
	public class CreateTimeSeriesTest : BaseAuthorizedTimeSeriesTest
	{
		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeCreateNewTimeSeries()
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
				.Then
				.CreatedTimeSeriesIsEqualToExpected();
		}

		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestDoubleCreateSameTimeSeries()
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
				.Then
				.CreatedTimeSeriesIsEqualToExpected();

			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected();
		}

		[Test]
		[Category(nameof(TestCategories.PriorityMedium))]
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		public void TestCreateNewTimeSeriesName(string name)
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = name,
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected();
		}

		[Test]
		[Category(nameof(TestCategories.PriorityMedium))]
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		public void TestCreateNewTimeSeriesDescription(string description)
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = description,
				Name = TestStringHelper.RandomLatinString(),
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected();
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		public void TestCreateNewTimeSeriesId(string id)
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				Id = id,
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected();
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		public void TestCreateNewTimeSeriesUserId(string id)
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				UserId = id,
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected();
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		public void TestCreateNewTimeSeriesInfluxId(string id)
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				InfluxId = id,
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected();
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		[TestCaseSource(typeof(DateToStringData), nameof(DateToStringData.PositiveDateTimeOffsetData))]
		[TestCaseSource(typeof(DateToStringData), nameof(DateToStringData.NegativeDateTimeOffsetData))]
		public void TestCreateNewTimeSeriesDateCreated(string date)
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				DateCreated = date,
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected();
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		[TestCaseSource(typeof(DateToStringData), nameof(DateToStringData.PositiveDateTimeOffsetData))]
		[TestCaseSource(typeof(DateToStringData), nameof(DateToStringData.NegativeDateTimeOffsetData))]
		public void TestCreateNewTimeSeriesDateModified(string date)
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				DateModified = date,
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected();
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
		public void TestCreateNewTimeSeriesSessionToken(string sessionToken)
		{
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
			};
			Given
				.NewTimeSeries(timeSeries)
				.UserSession(sessionToken)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.ResponseHasCode(HttpStatusCode.Unauthorized);
		}
	}
}
