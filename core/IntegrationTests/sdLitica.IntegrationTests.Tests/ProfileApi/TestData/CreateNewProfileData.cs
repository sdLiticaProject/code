using System;
using System.Collections.Generic;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.TestData
{
    public class CreateNewProfileData
    {
        public static IEnumerable<TestCaseData> NegativeNewProfileData
        {
            get
            {
                yield return new TestCaseData(new TestUserModel
                {
                    Email = null,
                    Password = TestStringHelper.RandomLatinString(),
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithNullEmail");

                yield return new TestCaseData(new TestUserModel
                {
                    Email = String.Empty,
                    Password = TestStringHelper.RandomLatinString(),
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithEmptyEmail");

                yield return new TestCaseData(new TestUserModel
                {
                    Password = String.Empty,
                    Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithNullPassword");

                yield return new TestCaseData(new TestUserModel
                {
                    Password = String.Empty,
                    Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithEmptyPassword");
                
                yield return new TestCaseData(new TestUserModel
                {
                    Password = TestStringHelper.RandomLatinString(),
                    Email = TestStringHelper.RandomLatinString(),
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithEmailWithoutAt");
                
                yield return new TestCaseData(new TestUserModel
                {
                    Password = TestStringHelper.RandomLatinString(),
                    Email = "@",
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithEmailOnlyAt");
                
                yield return new TestCaseData(new TestUserModel
                {
                    Password = TestStringHelper.RandomLatinString(),
                    Email = TestStringHelper.RandomLatinString() + "@" + TestStringHelper.RandomLatinString() + "@example.com",
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithEmailDoubleAt");
                
                yield return new TestCaseData(new TestUserModel
                {
                    Password = TestStringHelper.RandomLatinString(),
                    Email = TestStringHelper.RandomLatinString() + "@",
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithEmailWithoutDomain");
                
                yield return new TestCaseData(new TestUserModel
                {
                    Password = TestStringHelper.RandomLatinString(5),
                    Email = TestStringHelper.RandomLatinString() + "@example.com",
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithPasswordLengthLessThenMin");
                
                yield return new TestCaseData(new TestUserModel
                {
                    Password = TestStringHelper.RandomLatinString(101),
                    Email = TestStringHelper.RandomLatinString() + "@example.com",
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithPasswordLengthMoreThenMax");
            }
        }
        
        public static IEnumerable<TestCaseData> PositiveNewProfileData
        {
            get
            {
                yield return new TestCaseData(new TestUserModel
                {
                    Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                    Password = TestStringHelper.RandomLatinString(),
                    FirstName = null,
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithNullFirstName");

                yield return new TestCaseData(new TestUserModel
                {
                    Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                    Password = TestStringHelper.RandomLatinString(),
                    FirstName = String.Empty,
                    LastName = TestStringHelper.RandomLatinString(),
                }).SetName("TestCreateNewProfileWithEmptyFirstName");

                yield return new TestCaseData(new TestUserModel
                {
                    Password = TestStringHelper.RandomLatinString(),
                    Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = null,
                }).SetName("TestCreateNewProfileWithNullLastName");

                yield return new TestCaseData(new TestUserModel
                {
                    Password = TestStringHelper.RandomLatinString(),
                    Email = $"{TestStringHelper.RandomLatinString()}@example.com",
                    FirstName = TestStringHelper.RandomLatinString(),
                    LastName = String.Empty,
                }).SetName("TestCreateNewProfileWithEmptyLastName");
            }
        }
    }
}