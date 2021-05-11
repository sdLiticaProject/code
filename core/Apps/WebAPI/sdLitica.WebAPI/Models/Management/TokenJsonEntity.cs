using System.Collections.Generic;
using sdLitica.WebAPI.Entities.Common;

namespace sdLitica.WebAPI.Models.Management
{
    /// <summary>
    /// token model in JSON represenation for REST API
    /// </summary>
    public class TokenJsonEntity : BaseApiModel
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// expires date in long format
        /// </summary>
        public long Expires { get; set; }

        /// <summary>
        /// Default costructor from actual model entity
        /// </summary>
        public TokenJsonEntity()
        {
        }

        /// <summary>
        /// List of API required links
        /// </summary>
        public List<EntityLinkModel> Links { get; set; }

        /// <summary>
        /// Method returns an entity Id to use for referencing
        /// entity in REST API
        /// </summary>
        /// <returns>enity refernce id for REST API</returns>
        public string GetApiUrlPrefix()
        {
            return Token.ToString();
        }
    }
}
