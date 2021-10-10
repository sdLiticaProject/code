using System.Net;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.Tests.ProfileApi.Helpers;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests
{
    /// <summary>
    /// This class represents a base test class for all other integration test
    /// classes
    /// </summary>
    public class BaseWithDefaultUserTest : BaseApiTest
    {
        [OneTimeSetUp]
        public void InitTestUser()
        {
            var facade = new ProfileApiFacade(
                Logger,
                Configuration.RootUrl);

            var testUser = new TestUserModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password,
            };
            
            var response = facade.PostCreateNewProfile(testUser);
            
            if (response.StatusCode == HttpStatusCode.Conflict)
                return;
            if (response.StatusCode == HttpStatusCode.Created)
            {
                Assert.IsEmpty(ProfileHelper.CompareUserProfiles(testUser, response.Map<TestUserModel>()));
                return ;
            }

            Assert.Fail("Test user was not created");
        }
    }
}