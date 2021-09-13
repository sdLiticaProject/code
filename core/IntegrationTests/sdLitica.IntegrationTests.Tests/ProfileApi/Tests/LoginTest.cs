using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.TestUtils;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Tests
{
    public class LoginTest : ProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestBasicLoginLogout()
        {
            new GivenStatement(Logger)
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = Configuration.UserName,
                    Password = Configuration.Password
                })
                .When
                .LoginRequestIsSend()
                .Then
                .LogoutIfSessionTokenIsPresent()
                .SessionTokenIsPresent();
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void DoubleLoginTest()
        {
            var userKey1 = "user1";
            var userKey2 = "user2";

            new GivenStatement(Logger)
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = Configuration.UserName,
                    Password = Configuration.Password
                }, userKey1)
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = Configuration.UserName,
                    Password = Configuration.Password
                }, userKey2)
                .When
                .LoginRequestIsSend(userKey1)
                .LoginRequestIsSend(userKey2)
                .Then
                .SessionTokenIsPresent(userKey1)
                .SessionTokenIsPresent(userKey2)
                .LogoutIfSessionTokenIsPresent(userKey1)
                .LogoutIfSessionTokenIsPresent(userKey2);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithNullCredentials()
        {
            new GivenStatement(Logger)
                .UserLoginCredentials(new TestLoginModel())
                .When
                .LoginRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.BadRequest);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithPasswordOnly()
        {
            new GivenStatement(Logger)
                .UserLoginCredentials(new TestLoginModel
                {
                    Password = Configuration.Password
                })
                .When
                .LoginRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.BadRequest);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithLoginOnly()
        {
            new GivenStatement(Logger)
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = Configuration.UserName
                })
                .When
                .LoginRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.BadRequest);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithBadCredentials()
        {
            new GivenStatement(Logger)
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = TestStringHelper.RandomLatinString(),
                    Password = TestStringHelper.RandomLatinString()
                })
                .When
                .LoginRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithBadPassword()
        {
            new GivenStatement(Logger)
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = Configuration.UserName,
                    Password = TestStringHelper.RandomLatinString()
                })
                .When
                .LoginRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithBadLogin()
        {
            new GivenStatement(Logger)
                .UserLoginCredentials(new TestLoginModel
                {
                    Email = TestStringHelper.RandomLatinString(),
                    Password = Configuration.UserName
                })
                .When
                .LoginRequestIsSend()
                .Then
                .ResponseHasCode(HttpStatusCode.Unauthorized);
        }
    }
}