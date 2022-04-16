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
	public class SchedulerCreateTest : BaseAuthorizedSchedulerTest
	{
		[Test]
		[TestCaseSource(typeof(CreateTriggerData), nameof(CreateTriggerData.PositiveData))]
		public void CreatePositiveTest(TestCreateNewTriggerModel createTrigger)
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
			createTrigger.MetadataId = Guid.Parse(metaId);
			Given
				.UserSession(Session)
				.NewTrigger(createTrigger)
				.When
				.CreateNewTriggerRequestIsSend()
				.Then
				.ResponseHasCode(HttpStatusCode.OK);
		}

		[Test]
		[TestCaseSource(typeof(CreateTriggerData), nameof(CreateTriggerData.NegativeData))]
		public void CreateNegativeTest(TestCreateNewTriggerModel createTrigger)
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
			createTrigger.MetadataId = Guid.Parse(metaId);
			Given
				.UserSession(Session)
				.NewTrigger(createTrigger)
				.When
				.CreateNewTriggerRequestIsSend()
				.Then
				.ResponseHasCode(HttpStatusCode.BadRequest);
		}
	}
}
