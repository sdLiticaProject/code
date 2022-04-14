using System;
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
	public class SchedulerPauseResumeTest : BaseAuthorizedSchedulerTest
	{
		[Test]
		public void PausePositiveTest()
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
				.PauseCreatedTrigger()
				.WithSuccess()
				.TimePassed(TimeSpan.FromMinutes(1))
				.Then
				.TriggerNotFiredInTime(metaId, TimeSpan.FromMinutes(1));
		}

		[Test]
		public void DoublePausePositiveTest()
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
				.PauseCreatedTrigger()
				.WithSuccess()
				.PauseCreatedTrigger()
				.WithSuccess()
				.TimePassed(TimeSpan.FromMinutes(1))
				.Then
				.TriggerNotFiredInTime(metaId, TimeSpan.FromMinutes(1));
		}

		[Test]
		public void ResumePositiveTest()
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
				.PauseCreatedTrigger()
				.WithSuccess()
				.ResumeCreatedTrigger()
				.WithSuccess()
				.TimePassed(TimeSpan.FromMinutes(1))
				.Then
				.TriggerFiredInTime(metaId, TimeSpan.FromMinutes(1));
		}

		[Test]
		public void DoubleResumePositiveTest()
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
				.PauseCreatedTrigger()
				.WithSuccess()
				.ResumeCreatedTrigger()
				.WithSuccess()
				.ResumeCreatedTrigger()
				.WithSuccess()
				.TimePassed(TimeSpan.FromMinutes(1))
				.Then
				.TriggerFiredInTime(metaId, TimeSpan.FromMinutes(1));
		}
	}
}
