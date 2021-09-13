using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Extensions
{
    public static class ProfileGivenExtension
    {
        public static GivenStatement UserLoginCredentials(this GivenStatement givenStatement,
            TestLoginModel loginCredentials, string testKey = null)
        {
            givenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Saving user credentials {loginCredentials}", givenStatement.GetType().Name);
            givenStatement.AddData(loginCredentials, testKey);
            return givenStatement;
        }
        
        public static GivenStatement NewUserData(this GivenStatement givenStatement,
            TestUserModel userModel, string testKey = null)
        {
            givenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Saving user model {userModel}", givenStatement.GetType().Name);
            givenStatement.AddData(userModel, testKey);
            return givenStatement;
        }
    }
}