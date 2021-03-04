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
                return AuthenticateResult.Fail("Request was not archestrated with proper Authorization header");
            }
            
            if (string.IsNullOrEmpty(authorization))
            {
                //TODO: Credentials are missing. Maybe we need to act here somehow
                return AuthenticateResult.Fail("Authorization header is empty or malformed");
            }

            if (((string)authorization)?.Split(' ').Length != 2)
            {
                //TODO: Credentials are missing. Maybe we need to act here somehow
                return AuthenticateResult.Fail("Authorization header is malformed. Expected structure is '<schema> <value>'");
            }

            IServiceProvider serviceProvider = Request.HttpContext.RequestServices;
            UserService userService = serviceProvider.GetService<UserService>();
            Guid userId = default(Guid);
            try
            {
                string schema = ((string)authorization)?.Split(' ', 2)[0]; 
                string token = ((string)authorization)?.Split(' ', 2)[1];

                if (String.Equals(schema, CustomAuthOptions.DefaultSchema, StringComparison.OrdinalIgnoreCase))
                {
                    UserToken userToken = await userService.GetByTokenAsync(token);
                    if (userToken == null || userToken.IsTokenExpired())
                        return AuthenticateResult.Fail("Authorization header contains invalid token");

                    await userService.ShiftTokenAsync(userToken);
                    userId = userToken.UserId;
                    Options.Schema = CustomAuthOptions.DefaultSchema;
                } 
                else if (String.Equals(schema, CustomAuthOptions.ApiKeyScheme, StringComparison.OrdinalIgnoreCase))
                {
                    UserApiKey userApiKey = await userService.GetByApiKeyAsync(token);
                    if (userApiKey == null)
                        return AuthenticateResult.Fail("Authorization header contains invalid API key");

                    userId = userApiKey.UserId;
                    Options.Schema = CustomAuthOptions.ApiKeyScheme;
                }
                else 
                {
                    return AuthenticateResult.Fail("Authorization header contains unknows schema");
                }
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Auth failed: {ex.Message}");
            }

            List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            List<ClaimsIdentity> identities = new List<ClaimsIdentity> { new ClaimsIdentity(claims, Options.Schema)};
            AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Schema);

            return AuthenticateResult.Success(ticket);
        }

    }
}
