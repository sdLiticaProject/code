using Newtonsoft.Json.Linq;
using sdLitica.Entities.Abstractions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace sdLitica.Entities.Analytics
{
    /// <summary>
    /// enum for status of operation.
    /// </summary>
    public enum OperationStatus
    {
        InProcess = 0,
        Complete = 1,
        Error = -1
    }


    /// <summary>
    /// Entity for metadata of analytics operation.
    /// </summary>
    [Table("USER_ANALYTICS_OPERATIONS")]
    public class UserAnalyticsOperation : Entity
    {

        public UserAnalyticsOperation()
        {
        }


        /// <summary>
        /// Name of operation to be performed. 
        /// </summary>
        [Column("OPERATION_NAME")]
        public string OperationName { get; set; }

        /// <summary>
        /// Id of time-series on which operation is performed
        /// </summary>
        [Column("TIMESERIES_EXTERNAL_ID")]
        public string TimeSeriesId { get; set; }


        /// <summary>
        /// Current status of operation
        /// </summary>
        [Column("STATUS")]
        public OperationStatus Status { get; set; }

        /// <summary>
        /// Arguments for analytical operation
        /// </summary>
        [Column("ARGUMENTS")]
        public JObject Arguments { get; set; }

    }
}
