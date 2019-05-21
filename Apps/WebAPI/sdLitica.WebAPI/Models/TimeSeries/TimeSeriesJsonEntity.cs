using System.Collections.Generic;
using sdLitica.WebAPI.Entities.Common;
using sdLitica.WebAPI.Models.TimeSeries;

namespace sdLitica.WebAPI.Models.TimeSeries
{
    public class TimeSeriesJsonEntity : BaseApiModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Default costructor from actual model entity
        /// </summary>
        public TimeSeriesJsonEntity()
        {
        }

        public IDictionary<string, string> Tags { get; set; }

        /// <summary>
        /// Method returns an entity Id to use for referencing
        /// entity in REST API
        /// </summary>
        /// <returns>enity refernce id for REST API</returns>
        public string getApiUrlPrefix()
        {
            return "data";
        }

        public string GetApiUrlPrefix()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// List of API required links
        /// </summary>
        public List<EntityLinkModel> Links { get; set; }
    }
}