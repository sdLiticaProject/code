using sdLitica.Entities.Abstractions;
using System;

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
    public class AnalyticsOperationRequest : Entity
    {

        public AnalyticsOperationRequest()
        {
        }



        public Guid Id { get; set; } // temporary

        /// <summary>
        /// Name of operation to be performed. 
        /// </summary>
        public string OpName { get; set; }

        /// <summary>
        /// Id of time-series on which operation is performed
        /// </summary>
        public string TimeSeriesId { get; set; }


        /// <summary>
        /// Current status of operation
        /// </summary>
        public OperationStatus Status { get; set; }


        // TODO: add arguments of operation

    }
}
