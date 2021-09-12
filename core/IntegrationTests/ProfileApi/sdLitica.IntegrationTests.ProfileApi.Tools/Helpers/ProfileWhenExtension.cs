using System;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.RestApiTestBase;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using Serilog;

namespace sdLitica.IntegrationTests.ProfileApi.Tools.Helpers
{
    public static class ProfileWhenExtension
    {
        private static ProfileApiFacade _facade;
        public static void Init(ILogger logger, BaseTestConfiguration configuration)
        {
            _facade = new ProfileApiFacade(
                logger,
                configuration.RootUrl);
        }
        
        public static WhenStatement LoginRequestIsSend(this WhenStatement whenStatement, string testKey = null)
        {
            var loginCredentials = whenStatement.GetGivenData<TestLoginModel>(testKey);

            whenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Logging in with {loginCredentials}", whenStatement.GetType().Name);

            var response = _facade.PostLogin(loginCredentials);
            whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
            try
            {
                var session = response.GetTokenFromResponse();

                whenStatement.GetStatementLogger()
                    .Information($"[{{ContextStatement}}] Got session {session}", whenStatement.GetType().Name);

                whenStatement.AddResultData(session, BddKeyConstants.SessionTokenKey + testKey);
            }
            catch (Exception e)
            {
                whenStatement.GetStatementLogger()
                    .Warning($"[{{ContextStatement}}] {e}", whenStatement.GetType().Name);

            }
            
            return whenStatement;
        }
    }
}