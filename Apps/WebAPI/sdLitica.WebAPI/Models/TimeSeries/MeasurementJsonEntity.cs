using System.Collections.Generic;
using sdLitica.WebAPI.Entities.Common;
using sdLitica.WebAPI.Models.TimeSeries;

namespace sdLitica.WebAPI.Models.TimeSeries
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