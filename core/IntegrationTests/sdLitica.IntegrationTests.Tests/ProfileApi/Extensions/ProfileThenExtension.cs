using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Helpers;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;
using Serilog;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Extensions
{
    public static class ProfileThenExtension
    {
        private static ProfileApiFacade _facade;

        public static void Init(ProfileApiFacade facade)
        {
            _facade = facade;
        }

        public static ThenStatement ResponseHasCode(this ThenStatement thenStatement, HttpStatusCode code)
        {
            thenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Expecting last response to have code '{code}'",
                    thenStatement.GetType().Name);

            thenStatement.GetThenData<HttpResponseMessage>(BddKeyConstants.LastHttpResponse).AssertError(code);

            return thenStatement;
        }
        
        public static ThenStatement CurrentUserIsEqualToExpected(this ThenStatement thenStatement)
        {
            thenStatement.GetStatementLogger()
                .Information("[{ContextStatement}] Comparing given user credentials with response from 'GetMe'",
                    thenStatement.GetType().Name);
            
            var currentUser = thenStatement.GetThenData<TestUserModel>(BddKeyConstants.CurrentUserResponse);
            var expectedUser = thenStatement.GetGivenData<TestLoginModel>();
            
            Assert.That(currentUser.Email, Is.EqualTo(expectedUser.Email));
            Assert.That(currentUser.Password, Is.Null, "Expected user password to be hidden");
            Assert.That(currentUser.Id, Is.Not.Null, "Expected current user to have any Id");
            
            return thenStatement;
        }
        
        public static ThenStatement CurrentUserIsEqualTo(this ThenStatement thenStatement, TestUserModel expectedUser)
        {
            thenStatement.GetStatementLogger()
                .Information("[{ContextStatement}] Comparing 'GetMe' response with expected user model",
                    thenStatement.GetType().Name);
            
            var currentUser = thenStatement.GetThenData<TestUserModel>(BddKeyConstants.CurrentUserResponse);
            
            Assert.IsEmpty(ProfileHelper.CompareUserProfiles(expectedUser, currentUser));
            
            return thenStatement;
        }

        public static ThenStatement SessionTokenIsInvalid(this ThenStatement thenStatement, string testKey = null)
        {
            thenStatement.GetStatementLogger()
                .Information("[{ContextStatement}] Looking for session token in the 'When' dictionary",
                    thenStatement.GetType().Name);

            var session = thenStatement.GetThenData<string>(BddKeyConstants.SessionTokenKey + testKey);

            _facade.GetMe(session).AssertError(HttpStatusCode.Unauthorized);

            return thenStatement;
        }
        
        public static ThenStatement SessionTokenIsPresent(this ThenStatement thenStatement, string testKey = null)
        {
            thenStatement.GetStatementLogger()
                .Information("[{ContextStatement}] Looking for session token in the 'When' dictionary",
                    thenStatement.GetType().Name);

            Assert.That(thenStatement.GetThenData<string>(BddKeyConstants.SessionTokenKey + testKey), Is.Not.Empty,
                "Expected to have session token, but found none.");

            return thenStatement;
        }

        public static ThenStatement LogoutIfSessionTokenIsPresent(this ThenStatement thenStatement,
            string testKey = null)
        {
            try
            {
                thenStatement.GetStatementLogger()
                    .Information(
                        "[{ContextStatement}] Trying to logout" + (testKey != null ? $" with test key {testKey}" : ""),
                        thenStatement.GetType().Name);
                var session = thenStatement.GetThenData<string>(BddKeyConstants.SessionTokenKey + testKey);
                _facade.PostLogout(session).AssertSuccess();
            }
            catch (KeyNotFoundException e)
            {
                thenStatement.GetStatementLogger()
                    .Warning($"[{{ContextStatement}}] Could not find session token in 'When' results dictionary. {e}",
                        thenStatement.GetType().Name);
            }
            catch (Exception e)
            {
                thenStatement.GetStatementLogger()
                    .Warning($"[{{ContextStatement}}] An error occured during logout. {e}",
                        thenStatement.GetType().Name);
            }
            return thenStatement;
        }
    }
}