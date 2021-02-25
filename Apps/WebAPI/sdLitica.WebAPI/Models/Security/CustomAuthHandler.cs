using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.PlatformCore;
using sdLitica.WebAPI.Models.Security;
using sdLitica.Entities.Management;

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
            if (Request.Path.HasValue && Request.Path.Value.ToLower().Equals(Options.LoginPath))
            {
                return AuthenticateResult.NoResult();
            }

            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                return AuthenticateResult.Fail("Auth failed :(");
            }
            
            if (string.IsNullOrEmpty(authorization))
            {
                //TODO: Credentials are missing. Maybe we need to act here somehow
                return AuthenticateResult.Fail("Auth failed :(");
            }

            IServiceProvider serviceProvider = Request.HttpContext.RequestServices;
            UserService userService = serviceProvider.GetService<UserService>();
            Guid userId = default(Guid);
            try
            {
                string schema = ((string)authorization)?.Split(' ', 2)[0]; 
                string token = ((string)authorization)?.Split(' ', 2)[1];

                if (schema.ToLower().Equals("cloudtoken"))
                {
                    UserToken userToken = await userService.GetByTokenAsync(token);
                    if (userToken == null || userToken.IsTokenExpired())
                        throw new UnauthorizedAccessException();

                    await userService.ShiftTokenAsync(userToken);
                    userId = userToken.UserId;
                } 
                else if (schema.ToLower().Equals("cloudapikey"))
                {
                    UserApiKey userApiKey = await userService.GetByApiKeyAsync(token);
                    if (userApiKey == null)
                        throw new UnauthorizedAccessException();

                    userId = userApiKey.UserId;
                } 
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Auth failed: {ex.Message}");
            }

            List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            List<ClaimsIdentity> identities = new List<ClaimsIdentity> { new ClaimsIdentity(claims, Options.Scheme)};
            AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }

    }
}
