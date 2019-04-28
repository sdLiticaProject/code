using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using sdLitica.Services.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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
            
            if (string.IsNullOrEmpty(authorization))
            {
                //TODO: Credentials are missing. Maybe we need to act here somehow
                return AuthenticateResult.Fail("Auth failed :(");
            }

            var serviceProvider = Request.HttpContext.RequestServices;
            var userService = serviceProvider.GetService<UserService>();
            var userId = default(Guid);
            try
            {
                string token = ((string)authorization)?.Split(' ', 2)[1];                
                var userToken = await userService.GetByTokenAsync(token);
                if (userToken == null || userToken.IsTokenExpired())
                    throw new UnauthorizedAccessException();

                await userService.ShiftTokenAsync(userToken);
                userId = userToken.UserId;
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Auth failed: {ex.Message}");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identities = new List<ClaimsIdentity> { new ClaimsIdentity(claims, Options.Scheme)};
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }

    }
}
