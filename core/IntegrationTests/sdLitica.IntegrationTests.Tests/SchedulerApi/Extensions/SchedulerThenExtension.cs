using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.SchedulerApi.Extensions
{
	public static class SchedulerThenExtension
	{
		private static SchedulerApiFacade _facade;
		private static ProfileApiFacade _profileApiFacade;

		public static void Init(SchedulerApiFacade facade, ProfileApiFacade profileApiFacade)
		{
			_facade = facade;
			_profileApiFacade = profileApiFacade;
		}

		public static ThenStatement TriggersCountIncreasedBy(this ThenStatement thenStatement,
			int count,
			string testKey = null)
		{
			int oldCount = -1;
			int newCount = -1;

			try
			{
				oldCount =
					thenStatement.GetResultData<int>(
						BddKeyConstants.TriggersCount + testKey);
			}
			catch (KeyNotFoundException e)
			{
				Assert.Fail("Was unable to get trigger info due to request failure.");
			}

			newCount = _facade.GetAllTriggers(
					thenStatement.GetGivenData<string>(BddKeyConstants.SessionTokenKey + testKey))
				.Map<List<TestGetTriggerModel>>().Count;

			Assert.That(newCount, Is.EqualTo(oldCount + count),
				$"Expected {oldCount + count} triggers to be present, but found {newCount}");

			return thenStatement;
		}

		public static ThenStatement TriggerFiresOnesInTime(this ThenStatement thenStatement,
			string triggerId,
			TimeSpan time,
			string testKey = null)
		{
			var triggers = _facade.GetAllTriggers(
					thenStatement.GetGivenData<string>(BddKeyConstants.SessionTokenKey + testKey))
				.Map<List<TestGetTriggerModel>>();

			var trigger = triggers.First(e => e.TriggerKey.Equals(triggerId.ToString()));

			if (!trigger.LastFireTime.HasValue)
			{
				Assert.Fail("Trigger didn't fire");
			}

			Assert.That(trigger.NextFireTime - trigger.LastFireTime, Is.EqualTo(time),
				$"Expected {triggerId} trigger to fire every {time}, but found {trigger.NextFireTime - trigger.LastFireTime}");

			return thenStatement;
		}

		public static ThenStatement TriggerFiredInTime(this ThenStatement thenStatement,
			string triggerId,
			TimeSpan time,
			string testKey = null)
		{
			var triggers = _facade.GetAllTriggers(
					thenStatement.GetGivenData<string>(BddKeyConstants.SessionTokenKey + testKey))
				.Map<List<TestGetTriggerModel>>();

			var trigger = triggers.First(e => e.TriggerKey.Equals(triggerId.ToString()));

			if (!trigger.LastFireTime.HasValue)
			{
				Assert.Fail("Trigger didn't fire");
			}

			Assert.That(trigger.LastFireTime, Is.GreaterThan(DateTimeOffset.UtcNow - time),
				$"Expected {triggerId} trigger to fire at {DateTimeOffset.UtcNow - time}, but found {trigger.LastFireTime}");

			return thenStatement;
		}

		public static ThenStatement TriggerNotFiredInTime(this ThenStatement thenStatement,
			string triggerId,
			TimeSpan time,
			string testKey = null)
		{
			var triggers = _facade.GetAllTriggers(
					thenStatement.GetGivenData<string>(BddKeyConstants.SessionTokenKey + testKey))
				.Map<List<TestGetTriggerModel>>();

			var trigger = triggers.First(e => e.TriggerKey.Equals(triggerId.ToString()));

			if(trigger.LastFireTime.HasValue)
			{
				Assert.That(trigger.LastFireTime, Is.LessThan(DateTimeOffset.UtcNow - time),
					$"Expected {triggerId} trigger to fire at {DateTimeOffset.UtcNow - time}, but found {trigger.LastFireTime}");
			}

			return thenStatement;
		}
	}
}
