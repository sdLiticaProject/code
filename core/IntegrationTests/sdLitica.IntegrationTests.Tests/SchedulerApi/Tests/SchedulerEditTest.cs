using System;
using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.SchedulerApi.Extensions;
using sdLitica.IntegrationTests.Tests.SchedulerApi.TestData;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;

namespace sdLitica.IntegrationTests.Tests.SchedulerApi.Tests
{
	public class SchedulerEditTest : BaseAuthorizedSchedulerTest
	{
		[Test]
		public void EditSmokePositiveTest()
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
				.EditCreatedTriggerRequestIsSend(new TestEditTriggerModel()
				{
					CronSchedule = "0 0/2 * * * ?",
					FetchUrl = Configuration.SchedulerTestDataUrl
				})
				.TimePassed(TimeSpan.FromMinutes(2))
				.WithSuccess()
				.Then
				.TriggerCompletedSuccessfully(metaId)
				.TriggerFiresOnesInTime(metaId, TimeSpan.FromMinutes(2));
		}

		[Test]
		[TestCaseSource(typeof(EditTriggerData), nameof(EditTriggerData.PositiveData))]
		public void EditPositiveTest(TestEditTriggerModel editTrigger)
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
				.EditCreatedTriggerRequestIsSend(editTrigger)
				.Then
				.ResponseHasCode(HttpStatusCode.OK);
		}

		[Test]
		[TestCaseSource(typeof(EditTriggerData), nameof(EditTriggerData.NegativeData))]
		public void EditNegativeTest(TestEditTriggerModel editTrigger)
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
				.EditCreatedTriggerRequestIsSend(editTrigger)
				.Then
				.ResponseHasCode(HttpStatusCode.BadRequest);
		}

		[Test]
		public void EditNonExistentTriggerTest()
		{
			Given
				.UserSession(Session)
				.When
				.EditTriggerRequestIsSend(Guid.NewGuid().ToString(), new TestEditTriggerModel()
				{
					CronSchedule = "0 0/2 * * * ?",
					FetchUrl = Configuration.SchedulerTestDataUrl
				})
				.Then
				.ResponseHasCode(HttpStatusCode.NotFound);
		}
	}
}
