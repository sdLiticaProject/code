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
	public class UpdateTimeSeriesMetadataByIdTests : BaseAuthorizedTimeSeriesTest
	{
		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestSmokeUpdateTimeSeriesMetadataById()
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
				.UpdateTimeSeriesRequestIsSend()
				.UpdateGivenUpdateTimeSeriesIdsFromModel(timeSeries)
				.UpdateTimeSeriesRequestIsSend()
				.GetAllTimeSeriesRequestIsSend()
				.Then
				.TimeSeriesIsPresentInUserTimeSeries(updatedTimeSeries);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestDoubleUpdateTimeSeriesMetadataById()
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
				.UpdateTimeSeriesRequestIsSend()
				.UpdateGivenUpdateTimeSeriesIdsFromModel(timeSeries)
				.UpdateTimeSeriesRequestIsSend()
				.GetAllTimeSeriesRequestIsSend()
				.Then
				.TimeSeriesIsPresentInUserTimeSeries(updatedTimeSeries);

			updatedTimeSeries.Name = TestStringHelper.RandomLatinString();
			updatedTimeSeries.Description = TestStringHelper.RandomLatinString();

			Given
				.UpdateTimeSeries(updatedTimeSeries)
				.UserSession(Session)
				.When
				.UpdateTimeSeriesRequestIsSend()
				.GetAllTimeSeriesRequestIsSend()
				.Then
				.TimeSeriesIsPresentInUserTimeSeries(updatedTimeSeries);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityHigh))]
		public void TestUpdateRemovedTimeSeriesMetadataById()
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
				.RemoveTimeSeriesRequestIsSend(timeSeries.Id)
				.WithSuccess()
				.UpdateTimeSeriesRequestIsSend()
				.Then
				.ResponseHasCode(HttpStatusCode.NotFound);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonSessionData), nameof(CommonSessionData.NegativeSessionData))]
		public void TestUpdateTimeSeriesMetadataByIdSessionToken(string sessionToken)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};

			Given
				.UpdateTimeSeries(updatedTimeSeries)
				.UserSession(sessionToken)
				.When
				.UpdateTimeSeriesRequestIsSend()
				.Then
				.ResponseHasCode(HttpStatusCode.Unauthorized);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityMedium))]
		[TestCaseSource(typeof(UpdateTestsSeriesStringData), nameof(UpdateTestsSeriesStringData.PositiveStringDataWithUpdate))]
		public void TestUpdateTimeSeriesNameWithUpdate(string name)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Name = name,
			};
			var timeSeries = new TestTimeSeriesMetadataModel
			{
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
		[Category(nameof(TestCategories.PriorityMedium))]
		[TestCaseSource(typeof(UpdateTestsSeriesStringData), nameof(UpdateTestsSeriesStringData.PositiveStringDataWithoutUpdate))]
		public void TestUpdateTimeSeriesNameWithoutUpdate(string name)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Name = name,
			};
			var timeSeries = new TestTimeSeriesMetadataModel
			{
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
				.TimeSeriesByIdIsEqualTo(timeSeries);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityMedium))]
		[TestCaseSource(typeof(UpdateTestsSeriesStringData), nameof(UpdateTestsSeriesStringData.PositiveStringDataWithUpdate))]
		public void TestUpdateTimeSeriesDescription(string description)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Description = description,
				Name = TestStringHelper.RandomLatinString(),
			};
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = updatedTimeSeries.Name,
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
		[Category(nameof(TestCategories.PriorityMedium))]
		[TestCaseSource(typeof(UpdateTestsSeriesStringData), nameof(UpdateTestsSeriesStringData.PositiveStringDataWithoutUpdate))]
		public void TestUpdateTimeSeriesDescriptionWithoutUpdate(string description)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Description = description,
				Name = TestStringHelper.RandomLatinString(),
			};
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = updatedTimeSeries.Name,
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
				.TimeSeriesByIdIsEqualTo(timeSeries);
		}

		[Test]
		[Category(nameof(TestCategories.PriorityLow))]
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		public void TestUpdateTimeSeriesId(string id)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				Id = id,
			};
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = updatedTimeSeries.Name,
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
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		public void TestUpdateTimeSeriesUserId(string id)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				UserId = id,
			};
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = updatedTimeSeries.Name,
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
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		public void TestUpdateTimeSeriesInfluxId(string id)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				InfluxId = id,
			};
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = updatedTimeSeries.Name,
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
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		[TestCaseSource(typeof(DateToStringData), nameof(DateToStringData.PositiveDateTimeOffsetData))]
		[TestCaseSource(typeof(DateToStringData), nameof(DateToStringData.NegativeDateTimeOffsetData))]
		public void TestUpdateTimeSeriesDateCreated(string date)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				DateCreated = date,
			};
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = updatedTimeSeries.Name,
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
		[TestCaseSource(typeof(CommonStringData), nameof(CommonStringData.PositiveStringData))]
		[TestCaseSource(typeof(DateToStringData), nameof(DateToStringData.PositiveDateTimeOffsetData))]
		[TestCaseSource(typeof(DateToStringData), nameof(DateToStringData.NegativeDateTimeOffsetData))]
		public void TestUpdateTimeSeriesDateModified(string date)
		{
			var updatedTimeSeries = new TestTimeSeriesMetadataModel
			{
				Name = TestStringHelper.RandomLatinString(),
				DateModified = date,
			};
			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Name = updatedTimeSeries.Name,
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
	}
}
