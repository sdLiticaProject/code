using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.RestApiTestBase;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    public class LoginTest : ProfileApiTest
    {
        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void TestBasicLoginLogout()
        {
            var response = _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess();
            var session = _facade.GetTokenFromResponse(response);
            Assert.That(session, Is.Not.Empty);
            _facade.PostLogout(session);
        }

        [Test]
        [Category(nameof(TestCategories.PriorityHigh))]
        public void DoubleLoginTest()
        {
            var response = _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess();
            var session = _facade.GetTokenFromResponse(response);
            Assert.That(session, Is.Not.Empty);

            response = _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess();
            var newSession = _facade.GetTokenFromResponse(response);
            Assert.That(newSession, Is.Not.EqualTo(session), "Expected to have different session tokens");
            _facade.PostLogout(session).AssertSuccess();
            _facade.PostLogout(newSession).AssertSuccess();
        }

        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithNullCredentials()
        {
            _facade.PostLogin(new TestLoginModel()).AssertError(HttpStatusCode.BadRequest);
        }
        
        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithPasswordOnly()
        {
            _facade.PostLogin(new TestLoginModel
            {
                Password = Configuration.Password
            }).AssertError(HttpStatusCode.BadRequest);
        }
        
        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithLoginOnly()
        {
            _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName
            }).AssertError(HttpStatusCode.BadRequest);
        }
        
        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithBadCredentials()
        {
            _facade.PostLogin(new TestLoginModel
            {
                Email = TestStringHelper.RandomLatinString(),
                Password = TestStringHelper.RandomLatinString()
            }).AssertError(HttpStatusCode.Unauthorized);
        }
        
        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithBadPassword()
        {
            _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = TestStringHelper.RandomLatinString()
            }).AssertError(HttpStatusCode.Unauthorized);
        }
        
        [Test]
        [Category(nameof(TestCategories.PriorityMedium))]
        public void LoginWithBadLogin()
        {
            _facade.PostLogin(new TestLoginModel
            {
                Password = Configuration.Password,
                Email = TestStringHelper.RandomLatinString()
            }).AssertError(HttpStatusCode.Unauthorized);
        }
    }
}