using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils;
using Serilog;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Extensions
{
    public static class ProfileGivenExtension
    {
        private static BaseTestConfiguration _configuration;

        public static void Init(BaseTestConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static GivenStatement UserLoginCredentials(this GivenStatement givenStatement,
            TestLoginModel loginCredentials, string testKey = null)
        {
            givenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Saving user credentials {loginCredentials}",
                    givenStatement.GetType().Name);
            givenStatement.AddData(loginCredentials, testKey);
            return givenStatement;
        }

        public static GivenStatement DefaultUserLoginCredentials(this GivenStatement givenStatement,
            string testKey = null)
        {
            var loginCredentials = new TestLoginModel
            {
                Email = _configuration.UserName,
                Password = _configuration.Password,
            };
            return givenStatement.UserLoginCredentials(loginCredentials, testKey);
        }

        public static GivenStatement UserSession(this GivenStatement givenStatement, string session,
            string testKey = null)
        {
            givenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Saving user session '{session}'", givenStatement.GetType().Name);

            givenStatement.AddData(session, BddKeyConstants.SessionTokenKey + testKey);
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

        public static GivenStatement NewApiKey(this GivenStatement givenStatement,
            TestUserApiKeyJsonEntity apiKey, string testKey = null)
        {
            givenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Saving api keys {apiKey}", givenStatement.GetType().Name);
            givenStatement.AddData(apiKey, BddKeyConstants.NewApiKey + testKey);
            return givenStatement;
        }

        public static GivenStatement ApiKeyToRemove(this GivenStatement givenStatement,
            string apiKey, string testKey = null)
        {
            givenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Saving api key description to remove {apiKey}", givenStatement.GetType().Name);
            givenStatement.AddData(apiKey, BddKeyConstants.ApiKeyToRemove + testKey);
            return givenStatement;
        }
    }
}