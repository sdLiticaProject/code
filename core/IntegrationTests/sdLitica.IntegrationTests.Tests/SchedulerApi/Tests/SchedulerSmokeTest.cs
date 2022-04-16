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
	public class SchedulerSmokeTest : BaseAuthorizedSchedulerTest
	{
		[Test]
		public void SmokePositiveTest()
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
				.Then
				.TriggersCountIncreasedBy(1);
		}

		[Test]
		public void SmokeTriggerFiredPositiveTest()
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
				.CreateNewTriggerRequestIsSend()
				.WithSuccess()
				.TimePassed(TimeSpan.FromMinutes(1))
				.Then
				.TriggerCompletedSuccessfully(metaId)
				.TriggerFiredInTime(metaId, TimeSpan.FromMinutes(1));
		}

		[Test]
		public void SmokeTriggerFiredWithBadUrlPositiveTest()
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
					FetchUrl = "https://example.com"
				})
				.When
				.CreateNewTriggerRequestIsSend()
				.WithSuccess()
				.TimePassed(TimeSpan.FromMinutes(1))
				.Then
				.TriggerCompletedWithError(metaId)
				.TriggerFiredInTime(metaId, TimeSpan.FromMinutes(1));
		}

		[Test]
		public void SmokeDoubleCreateTriggerNegativeTest()
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
				.CreateNewTriggerRequestIsSend()
				.WithCode(HttpStatusCode.BadRequest)
				.Then
				.TriggersCountIncreasedBy(1);
		}
	}
}
