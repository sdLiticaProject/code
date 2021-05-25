using System;
using Microsoft.AspNetCore.Authentication;
using sdLitica.WebAPI.Security;

namespace sdLitica.WebAPI.Models.Security
{
    /// <summary>
    /// Custom authentication extension
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// Custom authentication extension method
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddCustomAuth(this AuthenticationBuilder builder, Action<CustomAuthOptions> configureOptions)
        {
            /// <summary>
            /// Add custom authentication scheme with custom options and custom handler
            /// </summary>
            /// <typeparam name="CustomAuthOptions"></typeparam>
            /// <typeparam name="CustomAuthHandler"></typeparam>
            /// <returns></returns>
            return builder
                    .AddScheme<CustomAuthOptions, CustomAuthHandler>(CustomAuthOptions.DefaultSchema, configureOptions)
                    .AddScheme<CustomAuthOptions, CustomAuthHandler>(CustomAuthOptions.ApiKeyScheme, configureOptions);
        }
    }
}
