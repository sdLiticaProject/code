using System.Collections.Generic;

namespace sdLitica.WebAPI.Entities.Common
{
    public class MeasurementJsonEntity : BaseApiModel
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
     
        /// <summary>
        /// Default costructor from actual model entity
        /// </summary>
        public MeasurementJsonEntity()
        {
            
        }
        
        /// <summary>
        /// Method returns an entity Id to use for referencing
        /// entity in REST API
        /// </summary>
        /// <returns>enity refernce id for REST API</returns>
        public string getApiUrlPrefix()
        {
            return Guid;
        }

        /// <summary>
        /// List of API required links
        /// </summary>
        public List<EntityLinkModel> Links { get; set; }
    }
}