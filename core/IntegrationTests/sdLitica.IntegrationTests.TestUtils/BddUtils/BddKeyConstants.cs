namespace sdLitica.IntegrationTests.TestUtils.BddUtils
{
    public static class BddKeyConstants
    {
        public static string SessionTokenKey = nameof(SessionTokenKey);
        public static string CreatedUserResponse = nameof(CreatedUserResponse);
        public static string CurrentUserResponse = nameof(CurrentUserResponse);
        public static string CurrentUserExpected = nameof(CurrentUserExpected);
        public static string LastHttpResponse = nameof(LastHttpResponse);
        public static string NewApiKey = nameof(NewApiKey);
        public static string UserApiKeys = nameof(UserApiKeys);
        public static string ApiKeyToRemove = nameof(ApiKeyToRemove);

        public static string TimeSeriesToCreate = nameof(TimeSeriesToCreate);
        public static string TimeSeriesToUpdate = nameof(TimeSeriesToUpdate);
        public static string CreatedTimeSeries = nameof(CreatedTimeSeries);
        public static string UserTimeSeries = nameof(UserTimeSeries);
        public static string UserTimeSeriesById = nameof(UserTimeSeriesById);
    }
}
