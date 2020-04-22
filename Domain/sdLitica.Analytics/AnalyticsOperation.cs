using sdLitica.Entities.Abstractions;
using System;

namespace sdLitica.Analytics
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


    //  At this moment, also used as its model in requests, messages. 

    /// <summary>
    /// Entity for metadata of analytics operation.
    /// </summary>
    public class AnalyticsOperation: Entity
    {

        public AnalyticsOperation()
        {
        }

        public void SetId()
        {
            Id = Guid.NewGuid();
        }


        public Guid Id { get; set; } // should have 'protected set'

        /// <summary>
        /// Name of operation to be performed. 
        /// </summary>
        public string OpName { get; set; } // should have 'protected set'

        /// <summary>
        /// Id of time-series on which operation is performed
        /// </summary>
        public string TimeSeriesId { get; set; }


        /// <summary>
        /// Current status of operation
        /// </summary>
        public OperationStatus Status { get; set; }


        // TODO: add parameters of operation
        //public Dictionary<string,string> Parameters { get; set; }
        //public List<OperationParameter> Parameters { get; set; }

    }
}
