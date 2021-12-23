using System;
using System.Collections.Generic;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.Tests.CommonTestData
{
    public class CommonSessionData
    {
        public static IEnumerable<TestCaseData> NegativeSessionData
        {
            get
            {
                yield return new TestCaseData(String.Empty).SetName("{m}WithEmptySession");

                yield return new TestCaseData(null).SetName("{m}WithNullSession");

                yield return new TestCaseData(TestStringHelper.RandomLatinString()).SetName("{m}WithLatinSession");

                yield return new TestCaseData(Guid.NewGuid().ToString()).SetName("{m}WithGuidSession");
            }
        }
    }
}