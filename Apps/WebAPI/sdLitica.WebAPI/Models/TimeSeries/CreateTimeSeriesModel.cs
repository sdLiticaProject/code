using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sdLitica.WebAPI.Models.TimeSeries
{
    public class CreateTimeSeriesModel
    {
        [Required]
        public string Name { get; set; }
    }
}
