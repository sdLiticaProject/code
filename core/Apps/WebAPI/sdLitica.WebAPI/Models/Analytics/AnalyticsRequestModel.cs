﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace sdLitica.WebAPI.Models.Analytics
{

    /// <summary>
    /// analytics request model
    /// </summary>
    public class AnalyticsRequestModel
    {

        /// <summary>
        /// Name of operation to be performed
        /// </summary>
        [Required]
        [Display(Name = nameof(OperationName))]
        public string OperationName { get; set; }

        /// <summary>
        /// Id of time-series in external store
        /// </summary>
        [Required]
        [Display(Name = nameof(TimeSeriesId))]
        public string TimeSeriesId { get; set; }

        /// <summary>
        /// Arguments of analytical operation
        /// </summary>
        [Required]
        [Display(Name = nameof(Arguments))]
        public JObject Arguments { get; set; }
    }
}
