using static sdLitica.Helpers.ReflectionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using sdLitica.WebAPI.Entities.Common;
using sdLitica.Entities.Management;

namespace sdLitica.WebAPI.Models.Management
{
    /// <summary>
    /// 
    /// </summary>
    public class UserJsonEntity : User, BaseApiModel
    {
        /// <summary>
        /// Default costructor from actual model entity
        /// </summary>
        /// <param name="profile"></param>
        public UserJsonEntity(User profile)
        {
            var props = typeof(User)
                            .GetProperties()
                            .Where(p => !p.GetIndexParameters().Any())
                            .Where(p => p.Name != GetPropertyName(() => profile.Id))
                            .Where(p => p.Name != GetPropertyName(() => profile.Password));
            foreach (var prop in props)
            {
                prop.SetValue(this, prop.GetValue(profile));
            }

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
            return null;
        }
    }
}
