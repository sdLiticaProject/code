using sdLitica.Entities.TimeSeries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sdLitica.WebAPI.Models.TimeSeries
{
    public class TimeSeriesMetadataModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string UserId { get; set; }
        public string InfluxId { get; set; }

        public TimeSeriesMetadataModel() { }
        public TimeSeriesMetadataModel(TimeSeriesMetadata timeSeriesMetadata)
        {
            Id = timeSeriesMetadata.Id.ToString();
            Name = timeSeriesMetadata.Name;
            Description = timeSeriesMetadata.Description;
            DateCreated = timeSeriesMetadata.DateCreated.ToString();
            DateModified = timeSeriesMetadata.DateModified.ToString();
            UserId = timeSeriesMetadata.UserId.ToString();
            InfluxId = timeSeriesMetadata.InfluxId.ToString();
        }
    }
}
