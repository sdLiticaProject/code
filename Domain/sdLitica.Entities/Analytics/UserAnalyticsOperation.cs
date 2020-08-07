﻿using sdLitica.Entities.Abstractions;
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
        public string OpName { get; set; }

        /// <summary>
        /// Id of time-series on which operation is performed
        /// </summary>
        [Column("TIMESERIES_INFLUX_ID")]
        public string TimeSeriesId { get; set; }


        /// <summary>
        /// Current status of operation
        /// </summary>
        [Column("STATUS")]
        public OperationStatus Status { get; set; }


        // TODO: add arguments of operation

    }
}
