using System;
using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.SchedulerApi.Extensions;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;

namespace sdLitica.IntegrationTests.Tests.SchedulerApi.Tests
{
	public class SchedulerDeleteTest : BaseAuthorizedSchedulerTest
	{
		[Test]
		public void DeletePositiveTest()
		{
			var createdTs = Given
				.NewTimeSeries(new TestTimeSeriesMetadataModel
				{
					Description = TestStringHelper.RandomLatinString(),
					Name = TestStringHelper.RandomLatinString(),
				})
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.WithSuccess()
				.Then
				.CreatedTimeSeriesIsEqualToExpected()
				.GetResultData<TestTimeSeriesMetadataModel>(BddKeyConstants.CreatedTimeSeries);

			var metaId = createdTs.Id;
			Given
				.UserSession(Session)
				.NewTrigger(new TestCreateNewTriggerModel()
				{
					CronSchedule = "0 0/1 * * * ?",
					MetadataId = Guid.Parse(metaId),
					FetchUrl = Configuration.SchedulerTestDataUrl
				})
				.When
				.GetCurrentTriggersCount()
				.WithSuccess()
				.CreateNewTriggerRequestIsSend()
				.WithSuccess()
				.DeleteCreatedTriggerRequestIsSend()
				.WithSuccess()
				.Then
				.TriggersCountIncreasedBy(0);
		}

		[Test]
		public void DoubleDeletePositiveTest()
		{
			var createdTs = Given
				.NewTimeSeries(new TestTimeSeriesMetadataModel
				{
					Description = TestStringHelper.RandomLatinString(),
					Name = TestStringHelper.RandomLatinString(),
				})
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.WithSuccess()
				.Then
				.CreatedTimeSeriesIsEqualToExpected()
				.GetResultData<TestTimeSeriesMetadataModel>(BddKeyConstants.CreatedTimeSeries);

			var metaId = createdTs.Id;
			Given
				.UserSession(Session)
				.NewTrigger(new TestCreateNewTriggerModel()
				{
					CronSchedule = "0 0/1 * * * ?",
					MetadataId = Guid.Parse(metaId),
					FetchUrl = Configuration.SchedulerTestDataUrl
				})
				.When
				.GetCurrentTriggersCount()
				.WithSuccess()
				.CreateNewTriggerRequestIsSend()
				.WithSuccess()
				.DeleteCreatedTriggerRequestIsSend()
				.WithSuccess()
				.DeleteCreatedTriggerRequestIsSend()
				.WithCode(HttpStatusCode.NotFound)
				.Then
				.TriggersCountIncreasedBy(0);
		}

		[Test]
		public void DeleteNonExistentTriggerTest()
		{
			Given
				.UserSession(Session)
				.When
				.DeleteTriggerRequestIsSend(Guid.NewGuid().ToString())
				.Then
				.ResponseHasCode(HttpStatusCode.NotFound);
		}

		[Test]
		public void DeleteWithTsPositiveTest()
		{
			var createdTs = Given
				.NewTimeSeries(new TestTimeSeriesMetadataModel
				{
					Description = TestStringHelper.RandomLatinString(),
					Name = TestStringHelper.RandomLatinString(),
				})
				.UserSession(Session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.WithSuccess()
				.Then
				.CreatedTimeSeriesIsEqualToExpected()
				.GetResultData<TestTimeSeriesMetadataModel>(BddKeyConstants.CreatedTimeSeries);

			var metaId = createdTs.Id;
			Given
				.UserSession(Session)
				.NewTrigger(new TestCreateNewTriggerModel()
				{
					CronSchedule = "0 0/1 * * * ?",
					MetadataId = Guid.Parse(metaId),
					FetchUrl = Configuration.SchedulerTestDataUrl
				})
				.When
				.GetCurrentTriggersCount()
				.WithSuccess()
				.CreateNewTriggerRequestIsSend()
				.WithSuccess()
				.RemoveTimeSeriesRequestIsSend(metaId)
				.WithSuccess()
				.Then
				.TriggersCountIncreasedBy(0);
		}
	}
}
