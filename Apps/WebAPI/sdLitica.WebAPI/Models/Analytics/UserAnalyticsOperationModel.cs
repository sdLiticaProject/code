using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sdLitica.WebAPI.Models.Analytics
{
    /// <summary>
    /// represents user's analytical operation
    /// </summary>
    public class UserAnalyticsOperationModel
    {
        /// <summary>
        /// Id of related UserAnalyticsOperation entity
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of operation to be performed
        /// </summary>
        [Required]
        [Display(Name = nameof(OperationName))]
        public string OperationName { get; set; }

        /// <summary>
        /// Id of time-series in external store
        /// </summary>
        [Display(Name = nameof(TimeSeriesId))]
        public string TimeSeriesId { get; set; }

        /// <summary>
        /// Status of analytical operation
        /// </summary>
        [Display(Name = nameof(TimeSeriesId))]
        public string Status { get; set; }

        /// <summary>
        /// Location of result of analytical operation
        /// </summary>
        [Display(Name = nameof(ResultLocation))]
        public string ResultLocation { get; set; }
    }
}
