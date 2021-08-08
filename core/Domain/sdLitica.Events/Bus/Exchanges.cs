namespace sdLitica.Events.Bus
{
    /// <summary>
    /// Exchanges available in the system as a whole
    /// </summary>
    public static class Exchanges
    {
        public const string TimeSeries = "TimeSeriesExchange";
        public const string Diagnostics = "DiagnosticsInfoExchange";
        public const string ModuleRegistrations = "ModuleRegistrationsExchange";
        public const string ModuleHeartbeats = "ModuleHeartbeatsExchange";
        public const string AnalysisResults = "AnalysisResultsExchange";

        public static string GetRpcQueue(string exchange) => $"{exchange}RpcQueue";
    }
}