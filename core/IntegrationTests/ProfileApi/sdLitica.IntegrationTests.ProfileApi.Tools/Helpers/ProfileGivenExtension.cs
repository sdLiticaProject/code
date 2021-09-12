using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.TestUtils.BddUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tools.Helpers
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
    }
}