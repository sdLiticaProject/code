using sdLitica.Analytics.Abstractions;
using sdLitica.WebAPI.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdLitica.WebAPI.Models.Analytics
{

    public class AnalyticsOperationModel : AnalyticsOperation, BaseApiModel
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
        public AnalyticsOperationModel()
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
            return "analytics_operation";
        }

        public string GetApiUrlPrefix()
        {
            return "analytics_operation"; //throw new System.NotImplementedException();
        }

        /// <summary>
        /// List of API required links
        /// </summary>
        public List<EntityLinkModel> Links { get; set; }
    }
}
