using System;
using System.Collections.Generic;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.TestData
{
    public class LogoutData
    {
        public static IEnumerable<TestCaseData> NegativeLogoutData
        {
            get
            {
                yield return new TestCaseData(String.Empty).SetName("TestLogoutWithEmptySession");

                yield return new TestCaseData(null).SetName("TestLogoutWithNullSession");

                yield return new TestCaseData(TestStringHelper.RandomLatinString()).SetName("TestLogoutWithLatinSession");

                yield return new TestCaseData(Guid.NewGuid().ToString()).SetName("TestLogoutWithGuidSession");
            }
        }
    }
}