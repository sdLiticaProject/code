using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;

namespace sdLitica.WebAPI.Security
{
    public class CustomAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultSchema = "cloudToken";
        public const string ApiKeySchema = "cloudApiKey"
        public string Scheme => DefaultScheme;
        public StringValues AuthKey { get; set; }
    }
}
