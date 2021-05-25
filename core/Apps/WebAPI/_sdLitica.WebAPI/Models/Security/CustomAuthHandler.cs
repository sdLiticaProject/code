using Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace sdLitica.WebAPI.Security
{
    public class CustomAuthHandler : AuthenticationHandler<CustomAuthOptions>
    {
        


        public CustomAuthHandler(IOptionsMonitor<CustomAuthOptions> options,
                                ILoggerFactory logger,
                                UrlEncoder encoder,
                                ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            
        }

        /// <summary>
        /// Handler for Auth async ovveride
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                return AuthenticateResult.Fail("Auth failed :(");
            }

            // If there are credentials that the filter understands, try to validate them.
            // If the credentials are bad, set the error result.
            if (string.IsNullOrEmpty(authorization))
            {
                //TODO: Credentials are missing. Maybe we need to act here somehow
                return AuthenticateResult.Fail("Auth failed :(");
            }

            // Create authenticated user
            string authorizationHeader = authorization;
            
            string token = authorizationHeader.Split(' ', 2)[1];

            // validate token
            string profileId = null;
            try
            {
                if (token == "fake-token-value")
                {
                    profileId = "0";
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
                
            }
            catch(Exception ex)
            {
                return AuthenticateResult.Fail($"Auth failed: {ex.Message}");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, profileId) };
            var identities = new List<ClaimsIdentity> { new ClaimsIdentity(claims, Options.Scheme)};
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }

    }
}
