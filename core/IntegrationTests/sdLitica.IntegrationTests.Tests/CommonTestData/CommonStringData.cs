using System;
using System.Collections.Generic;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.Tests.CommonTestData
{
	public class CommonStringData
	{
		public static IEnumerable<TestCaseData> PositiveStringData
		{
			get
			{
				yield return new TestCaseData(String.Empty).SetName("{m}WithEmptyValue");

				yield return new TestCaseData(null).SetName("{m}WithNullValue");

				yield return new TestCaseData(TestStringHelper.RandomLatinString()).SetName("{m}WithLatinValue");

				yield return new TestCaseData(TestStringHelper.RandomCyrillicString()).SetName("{m}WithCyrillicValue");

				yield return new TestCaseData(TestStringHelper.RandomNumberString()).SetName("{m}WithNumbersValue");

				yield return new TestCaseData(TestStringHelper.RandomMixedString()).SetName("{m}WithMixedValue");

				yield return new TestCaseData(TestStringHelper.RandomSpecCharactersString()).SetName(
					"{m}WithSpecCharactersValue");

				yield return new TestCaseData(Guid.NewGuid().ToString()).SetName("{m}WithGuidValue");
			}
		}
	}
}
