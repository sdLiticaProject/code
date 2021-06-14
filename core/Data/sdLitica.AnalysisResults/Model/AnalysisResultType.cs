namespace sdLitica.AnalysisResults.Model
{
    public enum AnalysisResultType
    {
        /// <summary>
        /// Result represents a single number
        /// </summary>
        Number,
        /// <summary>
        /// Result represents a pair
        /// of some value/estimation and time point
        /// </summary>
        Pair,
        /// <summary>
        /// Result represents a unique id
        /// of new resulting timeseries,
        /// which should be stored in InfluxDB 
        /// </summary>
        TimeSeries,
        /// <summary>
        /// Result represents an math equation.
        /// TODO: probably, it should be MathML format 
        /// </summary>
        Formula,
    }
}