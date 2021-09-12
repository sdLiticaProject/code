using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using sdLitica.IntegrationTests.RestApiTestBase;
using sdLitica.IntegrationTests.TestUtils.BddUtils;
using Serilog;

namespace sdLitica.IntegrationTests.ProfileApi.Tools.Helpers
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
                .Information($"[{{ContextStatement}}] Expecting last response to have code '{code}'", thenStatement.GetType().Name);

            thenStatement.GetThenData<HttpResponseMessage>(BddKeyConstants.LastHttpResponse).AssertError(code);
            
            return thenStatement;
        }

        
        public static ThenStatement SessionTokenIsPresent(this ThenStatement thenStatement, string testKey = null)
        {
            thenStatement.GetStatementLogger()
                .Information("[{ContextStatement}] Looking for session token in the 'When' dictionary", thenStatement.GetType().Name);
            
            Assert.That(thenStatement.GetThenData<string>(BddKeyConstants.SessionTokenKey + testKey), Is.Not.Empty,
                "Expected to have session token, but found none.");
            
            return thenStatement;
        }

        public static ThenStatement LogoutIfSessionTokenIsPresent(this ThenStatement thenStatement, string testKey = null)
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