using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.TimeSeriesApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models;
using sdLitica.IntegrationTests.TestUtils.Facades.TimeSeriesApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.SchedulerApi.Tests
{
	public class SchedulerSmokeTest : SchedulerApiTest
	{
		[Test]
		public void SmokePositiveTest()
		{
			var session = ProfileFacade.PostLogin(new TestLoginModel()
			{
				Email = Configuration.UserName,
				Password = Configuration.Password
			}).GetTokenFromResponse();

			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};
			var createdTs = Given
				.NewTimeSeries(timeSeries)
				.UserSession(session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected()
				.GetResultData<TestTimeSeriesMetadataModel>(BddKeyConstants.CreatedTimeSeries);

			Logger.Debug("Got {TimeSeries} as created meta.", createdTs);

			var metaId = createdTs.Id;

			Facade.PostCreateTrigger(session, new TestCreateNewTriggerModel()
			{
				CronSchedule = "0 0/1 * * * ?",
				MetadataId = Guid.Parse(metaId)
			}).AssertSuccess();

			Thread.Sleep(TimeSpan.FromMinutes(1));

			var triggers = Facade.GetAllTriggers(session).AssertSuccess().Map<List<TestGetTriggerModel>>();

			ProfileFacade.PostLogout(session);

			Assert.That(triggers.Count(e => e.TriggerKey.Equals(metaId)), Is.EqualTo(1));
			Assert.That(triggers[0].LastFireTime.HasValue);
			Assert.That(triggers[0].LastFireTime.Value, Is.GreaterThan(DateTimeOffset.UtcNow.AddMinutes(-2)));
		}

		[Test]
		public void SmokeDoubleCreateTriggerNegativeTest()
		{
			var session = ProfileFacade.PostLogin(new TestLoginModel()
			{
				Email = Configuration.UserName,
				Password = Configuration.Password
			}).GetTokenFromResponse();

			var triggersOld = Facade.GetAllTriggers(session).AssertSuccess().Map<List<TestGetTriggerModel>>();

			var timeSeries = new TestTimeSeriesMetadataModel
			{
				Description = TestStringHelper.RandomLatinString(),
				Name = TestStringHelper.RandomLatinString(),
			};
			var createdTs = Given
				.NewTimeSeries(timeSeries)
				.UserSession(session)
				.When
				.CreateNewTimeSeriesRequestIsSend()
				.Then
				.CreatedTimeSeriesIsEqualToExpected()
				.GetResultData<TestTimeSeriesMetadataModel>(BddKeyConstants.CreatedTimeSeries);

			Logger.Debug("Got {TimeSeries} as created meta.", createdTs);

			var metaId = createdTs.Id;

			Facade.PostCreateTrigger(session, new TestCreateNewTriggerModel()
			{
				CronSchedule = "0 0/1 * * * ? * ",
				MetadataId = Guid.Parse(metaId)
			}).AssertSuccess();

			Facade.PostCreateTrigger(session, new TestCreateNewTriggerModel()
			{
				CronSchedule = "0 0/1 * 1/1 * ? * ",
				MetadataId = Guid.Parse(metaId)
			}).AssertError(HttpStatusCode.BadRequest);

			var triggers = Facade.GetAllTriggers(session).AssertSuccess().Map<List<TestGetTriggerModel>>();

			ProfileFacade.PostLogout(session);

			Assert.That(triggers.Count, Is.EqualTo(triggersOld.Count + 1));
		}
	}
}
