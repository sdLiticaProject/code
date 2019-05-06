using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace sdLitica.WebAPI.Security
{
    public class CustomAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "cloudToken";
        public string Scheme => DefaultScheme;
        public StringValues AuthKey { get; set; }
        public PathString LoginPath { get; set; } = "/api/v1/profile/login";
    }
}
