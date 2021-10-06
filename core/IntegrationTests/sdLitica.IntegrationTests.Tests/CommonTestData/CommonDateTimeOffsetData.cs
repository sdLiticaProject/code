using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace sdLitica.IntegrationTests.Tests.CommonTestData
{
	public class CommonDateTimeOffsetData
	{
		public static IEnumerable<TestCaseData> PositiveDateTimeOffsetData
		{
			get
			{
				yield return new TestCaseData(DateTimeOffset.MaxValue).SetName("{m}WithMaxDateValue");

				yield return new TestCaseData(DateTimeOffset.MinValue).SetName("{m}WithMinDateValue");

				yield return new TestCaseData(DateTimeOffset.UtcNow.AddDays(-1)).SetName("{m}WithPastUtcDateValue");

				yield return new TestCaseData(DateTimeOffset.Now.AddDays(-1).ToLocalTime()).SetName("{m}WithPastLocalDateValue");
			}
		}

		public static IEnumerable<TestCaseData> NegativeDateTimeOffsetData
		{
			get
			{
				yield return new TestCaseData(null).SetName("{m}WithNullDateValue");
			}
		}
	}
}
