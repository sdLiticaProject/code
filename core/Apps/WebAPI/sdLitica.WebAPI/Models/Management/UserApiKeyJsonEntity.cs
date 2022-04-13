using System.Collections.Generic;
using sdLitica.CommonApiServices.ApiVersion.JsonAPI;
using sdLitica.Entities.Management;

namespace sdLitica.WebAPI.Models.Management
{
    /// <summary>
    /// User API Key in JSON represenation for REST API
    /// </summary>
    public class UserApiKeyJsonEntity : BaseApiModel
    {

        /// <summary>
        /// Unique key Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Api Key value
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Key description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Default costructor from actual model entity
        /// </summary>
        public UserApiKeyJsonEntity(UserApiKey apiKey)
        {
            Id = apiKey.Id.ToString();
            Key = apiKey.APIKey;
            Description = apiKey.Description;
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
            return Id;
        }
    }
}
