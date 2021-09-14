﻿using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
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

        private static string GetSessionFromData(this WhenStatement whenStatement, string testKey = null)
        {
            string session;
            try
            {
                session = whenStatement.GetResultData<string>(BddKeyConstants.SessionTokenKey + testKey);
                return session;
            }
            catch
            {
                whenStatement.GetStatementLogger()
                    .Information("[{ContextStatement}] Could not find user session in 'When' data, looking in 'Given'",
                        whenStatement.GetType().Name);
            }

            try
            {
                session = whenStatement.GetGivenData<string>(BddKeyConstants.SessionTokenKey + testKey);
            }
            catch
            {
                whenStatement.GetStatementLogger()
                    .Information(
                        "[{ContextStatement}] Could not find user session in 'Given' data, looking in last response data",
                        whenStatement.GetType().Name);
                session = whenStatement.GetResultData<HttpResponseMessage>(BddKeyConstants.LastHttpResponse + testKey)
                    .GetTokenFromResponse();
            }

            return session;
        }

        public static void Init(ProfileApiFacade facade)
        {
            _facade = facade;
        }

        public static WhenStatement LoginRequestIsSend(this WhenStatement whenStatement, string testKey = null)
        {
            var loginCredentials = whenStatement.GetGivenData<TestLoginModel>(testKey);

            whenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Logging in with {loginCredentials}",
                    whenStatement.GetType().Name);

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

        public static WhenStatement LogoutRequestIsSend(this WhenStatement whenStatement, string testKey = null)
        {
            var session = whenStatement.GetSessionFromData(testKey);

            whenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Closing session '{session}'", whenStatement.GetType().Name);

            var response = _facade.PostLogout(session);
            whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

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

        public static WhenStatement GetCurrentUserRequestIsSend(this WhenStatement whenStatement, string testKey = null)
        {
            whenStatement.GetStatementLogger()
                .Information("[{ContextStatement}] Getting current user", whenStatement.GetType().Name);

            var session = whenStatement.GetSessionFromData(testKey);

            var response = _facade.GetMe(session);
            whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);
            try
            {
                var currentUser = response.MapAndLog<TestUserModel>();
                whenStatement.GetStatementLogger()
                    .Information($"[{{ContextStatement}}] Got user {currentUser}", whenStatement.GetType().Name);

                whenStatement.AddResultData(currentUser, BddKeyConstants.CurrentUserResponse + testKey);
            }
            catch
            {
                whenStatement.GetStatementLogger()
                    .Information("[{ContextStatement}] Could not find current user in response", whenStatement.GetType().Name);
            }

            return whenStatement;
        }
        
        public static WhenStatement UpdateUserRequestIsSend(this WhenStatement whenStatement, TestUserUpdateModel updateModel, string testKey = null)
        {
            whenStatement.GetStatementLogger()
                .Information("[{ContextStatement}] Getting current user", whenStatement.GetType().Name);

            var session = whenStatement.GetSessionFromData(testKey);

            var response = _facade.PostUpdateProfileNames(session, updateModel);
            whenStatement.AddResultData(response, BddKeyConstants.LastHttpResponse + testKey);

            return whenStatement;
        }
    }
}