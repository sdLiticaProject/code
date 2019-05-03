using System.Collections.Generic;

namespace sdLitica.WebAPI.Entities.Common
{
    public class TimeseriesJsonEntity : BaseApiModel
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
        public TimeseriesJsonEntity()
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

        /// <summary>
        /// List of API required links
        /// </summary>
        public List<EntityLinkModel> Links { get; set; }
    }
}