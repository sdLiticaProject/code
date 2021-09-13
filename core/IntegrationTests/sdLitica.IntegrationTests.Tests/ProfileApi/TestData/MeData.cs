using System;
using System.Collections.Generic;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.TestData
{
    public class MeData
    {
        public static IEnumerable<TestCaseData> NegativeMyProfileData
        {
            get
            {
                yield return new TestCaseData(String.Empty).SetName("TestMeWithEmptySession");

                yield return new TestCaseData(null).SetName("TestMeWithNullSession");

                yield return new TestCaseData(TestStringHelper.RandomLatinString()).SetName("TestMeWithLatinSession");

                yield return new TestCaseData(Guid.NewGuid().ToString()).SetName("TestMeWithGuidSession");
            }
        }
    }
}