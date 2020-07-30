using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sdLitica.WebAPI.Models.Analytics
{

    /// <summary>
    /// analytics request model
    /// </summary>
    public class AnalyticsRequestModel
    {

        [Required]
        [Display(Name = nameof(OperationName))]
        public string OperationName { get; set; }

        [Required]
        [Display(Name = nameof(TimeSeriesId))]
        public string TimeSeriesId { get; set; }
    }
}
