using System;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;
using Serilog;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Extensions
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
        
        public static WhenStatement CreateUserRequestIsSend(this WhenStatement whenStatement, string testKey = null)
        {
            var userModel = whenStatement.GetGivenData<TestUserModel>(testKey);

            whenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Creating user {userModel}", whenStatement.GetType().Name);

            var response = _facade.PostCreateNewProfile(userModel);
            whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
            try
            {
                var createdUser = response.MapAndLog<TestUserModel>();
                whenStatement.GetStatementLogger()
                    .Information($"[{{ContextStatement}}] Got new user {createdUser}", whenStatement.GetType().Name);

                whenStatement.AddResultData(createdUser, BddKeyConstants.CreatedUserResponse + testKey);
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