using System.Collections.Generic;

namespace sdLitica.WebAPI.Entities.Common
{
    public class TimeseriesDataJsonEntity : BaseApiModel
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
        public TimeseriesDataJsonEntity()
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

        /// <summary>
        /// List of API required links
        /// </summary>
        public List<EntityLinkModel> Links { get; set; }
    }
}