using System;
using System.Collections.Generic;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.TestData
{
    public class ApiKeysData
    {
        public static IEnumerable<TestCaseData> NegativeGetApiKeysSessionData
        {
            get
            {
                yield return new TestCaseData(String.Empty).SetName("TestGetApiKeysWithEmptySession");

                yield return new TestCaseData(null).SetName("TestGetApiKeysWithNullSession");

                yield return new TestCaseData(TestStringHelper.RandomLatinString()).SetName("TestGetApiKeysWithLatinSession");

                yield return new TestCaseData(Guid.NewGuid().ToString()).SetName("TestGetApiKeysWithGuidSession");
            }
        }
        
        public static IEnumerable<TestCaseData> NegativePostApiKeysSessionData
        {
            get
            {
                yield return new TestCaseData(String.Empty).SetName("TestPostApiKeysWithEmptySession");

                yield return new TestCaseData(null).SetName("TestPostApiKeysWithNullSession");

                yield return new TestCaseData(TestStringHelper.RandomLatinString()).SetName("TestPostApiKeysWithLatinSession");

                yield return new TestCaseData(Guid.NewGuid().ToString()).SetName("TestPostApiKeysWithGuidSession");
            }
        }
        
        public static IEnumerable<TestCaseData> NegativeDeleteApiKeysSessionData
        {
            get
            {
                yield return new TestCaseData(String.Empty).SetName("TestDeleteApiKeysWithEmptySession");

                yield return new TestCaseData(null).SetName("TestDeleteApiKeysWithNullSession");

                yield return new TestCaseData(TestStringHelper.RandomLatinString()).SetName("TestDeleteApiKeysWithLatinSession");

                yield return new TestCaseData(Guid.NewGuid().ToString()).SetName("TestDeleteApiKeysWithGuidSession");
            }
        }
    }
}