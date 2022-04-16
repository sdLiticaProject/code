using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace sdLitica.CommonApiServices.Security
{
    /// <summary>
    /// This class contains set of project-specific auth-specific 
    /// contstants
    /// </summary>
    public class CustomAuthOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Default auth schema used when user is accessing
        /// API's of the system with the token retrieven 
        /// with standard login flow
        /// </summary>
        public const string DefaultSchema = "cloudToken";

        /// <summary>
        /// Auth schema which is used by an end-user when
        /// requests are authorized with end-user API Key
        /// </summary>
        public const string ApiKeyScheme = "apiKeyToken";

        /// <summary>
        /// Currently selected schema
        /// </summary>
        /// <value></value>
        public string Schema { get; set; }
        
        /// <summary>
        /// Authentication key which can be either value of a 
        /// token or API key depenting on the specified schema
        /// </summary>
        /// <value></value>
        public StringValues AuthKey { get; set; }

        /// <summary>
        /// Default application login path used to retrieve 
        /// a user token
        /// </summary>
        /// <value></value>
        public PathString LoginPath { get; set; } = "/api/v1/profile/login";

        /// <summary>
        /// Default options constructor
        /// </summary>
        public CustomAuthOptions()
        {
            Schema = DefaultSchema;
        }
    }
}
