using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.CommonTestData;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.TestData
{
	public class DateToStringData
	{
		public static IEnumerable<TestCaseData> PositiveDateTimeOffsetData =>
			CommonDateTimeOffsetData.PositiveDateTimeOffsetData
				.Select(testCase => new TestCaseData(testCase.Arguments[0]!.ToString()).SetName(testCase.TestName));

		public static IEnumerable<TestCaseData> NegativeDateTimeOffsetData =>
			CommonDateTimeOffsetData.NegativeDateTimeOffsetData
				.Select(testCase => new TestCaseData(testCase.Arguments[0]?.ToString()).SetName(testCase.TestName));

	}
}
