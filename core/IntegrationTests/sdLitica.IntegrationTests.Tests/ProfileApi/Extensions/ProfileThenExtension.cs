using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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

        public static void Init(ILogger logger, BaseTestConfiguration configuration)
        {
            _facade = new ProfileApiFacade(
                logger,
                configuration.RootUrl);
        }

        public static ThenStatement ResponseHasCode(this ThenStatement thenStatement, HttpStatusCode code)
        {
            thenStatement.GetStatementLogger()
                .Information($"[{{ContextStatement}}] Expecting last response to have code '{code}'",
                    thenStatement.GetType().Name);

            thenStatement.GetThenData<HttpResponseMessage>(BddKeyConstants.LastHttpResponse).AssertError(code);

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

        public static ThenStatement TestUserIsCreatedOrPresent(this ThenStatement thenStatement, string testKey = null)
        {
            var response = thenStatement.GetThenData<HttpResponseMessage>(BddKeyConstants.LastHttpResponse);
            if (response.StatusCode == HttpStatusCode.Conflict)
                return thenStatement;
            if (response.StatusCode == HttpStatusCode.Created)
            {
                Assert.IsEmpty(ProfileHelper.CompareUserProfiles(
                    thenStatement.GetGivenData<TestUserModel>(),
                    thenStatement.GetThenData<TestUserModel>(BddKeyConstants.CreatedUserResponse)));
                return thenStatement;
            }

            Assert.Fail("Test user was not created");
            return thenStatement;
        }
    }
}