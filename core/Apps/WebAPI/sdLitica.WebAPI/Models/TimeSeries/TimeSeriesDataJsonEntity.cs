using System.Collections.Generic;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;
using sdLitica.WebAPI.Entities.Common;
using sdLitica.WebAPI.Models.TimeSeries;

namespace sdLitica.WebAPI.Models.TimeSeries
{
    public class TimeSeriesDataJsonEntity : BaseApiModel
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Timeseries list of fields
        /// </summary>
        public IDictionary<string, object> Fields { get; set; }

        /// <summary>
        /// Timeseries list of tags
        /// </summary>
        public IDictionary<string, string> Tags { get; set; }

        /// <summary>
        /// Default costructor from actual model entity
        /// </summary>
        public TimeSeriesDataJsonEntity()
        {
        }

        /// <summary>
        /// Method returns an entity Id to use for referencing
        /// entity in REST API
        /// </summary>
        /// <returns>enity refernce id for REST API</returns>
        public string getApiUrlPrefix()
        {
            return "";
        }

        public string GetApiUrlPrefix()
        {
            return "";// throw new System.NotImplementedException();
        }

        /// <summary>
        /// List of API required links
        /// </summary>
        [JsonIgnore]
        public List<EntityLinkModel> Links { get; set; }
    }
}