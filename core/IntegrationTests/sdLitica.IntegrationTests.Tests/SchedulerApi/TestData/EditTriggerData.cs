using System.Collections.Generic;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models;

namespace sdLitica.IntegrationTests.Tests.SchedulerApi.TestData
{
	public class EditTriggerData
	{

		public static IEnumerable<TestCaseData> PositiveData
		{
			get
			{
				yield return new TestCaseData(new TestEditTriggerModel
				{
					CronSchedule = "0 0/1 * * * ?",
					FetchUrl = "https://example.com"
				}).SetName("{m}TriggerEvery2Minutes");
				yield return new TestCaseData(new TestEditTriggerModel
				{
					CronSchedule = "0 0/20 * * * ?",
					FetchUrl = "https://example.com"
				}).SetName("{m}TriggerEvery20Minutes");
				yield return new TestCaseData(new TestEditTriggerModel
				{
					CronSchedule = "0 10,44 14 ? 3 WED",
					FetchUrl = "https://example.com"
				}).SetName("{m}TriggerEveryDifficultCron");
				yield return new TestCaseData(new TestEditTriggerModel
				{
					CronSchedule = "0 0 12 * * ?",
					FetchUrl = "https://example.com"
				}).SetName("{m}TriggerEveryDayAt12");
				yield return new TestCaseData(new TestEditTriggerModel
				{
					CronSchedule = "0/1 0 0 * * ?",
					FetchUrl = "https://example.com"
				}).SetName("{m}TriggerEverySecond");
			}
		}

		public static IEnumerable<TestCaseData> NegativeData
		{
			get
			{
				yield return new TestCaseData(new TestEditTriggerModel
				{
					CronSchedule = "abcade",
					FetchUrl = "https://example.com"
				}).SetName("{m}WithBadScheduleValue");
				yield return new TestCaseData(new TestEditTriggerModel
				{
					CronSchedule = "0 0/1 * * * ?",
					FetchUrl = "abcddefea"
				}).SetName("{m}WithBadUrlValue");
			}
		}
	}
}
