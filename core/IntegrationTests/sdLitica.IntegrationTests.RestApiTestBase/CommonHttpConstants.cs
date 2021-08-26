namespace sdLitica.IntegrationTests.RestApiTestBase
{
    public static class CommonHttpConstants
    {
        public static readonly string ApplicationJsonMedia = "application/json";
        public static readonly string AuthorizationHeader = "cloudToken";
        public static readonly string AuthorizationTokenResponse = "$.entity.token";
        public static readonly string AuthorizationTokenExpirationDateResponse = "$.entity.expires";
    }
}