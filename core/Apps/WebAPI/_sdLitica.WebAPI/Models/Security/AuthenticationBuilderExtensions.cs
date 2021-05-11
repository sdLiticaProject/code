using Microsoft.AspNetCore.Authentication;
using System;

namespace sdLitica.WebAPI.Security
{
    public static class AuthenticationBuilderExtensions
    {
        // Custom authentication extension method
        public static AuthenticationBuilder AddCustomAuth(this AuthenticationBuilder builder, Action<CustomAuthOptions> configureOptions)
        {
            // Add custom authentication scheme with custom options and custom handler
            return builder.AddScheme<CustomAuthOptions, CustomAuthHandler>(CustomAuthOptions.DefaultScheme, configureOptions);
        }
    }
}
