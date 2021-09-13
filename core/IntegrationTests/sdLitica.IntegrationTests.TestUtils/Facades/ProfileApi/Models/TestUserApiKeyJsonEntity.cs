using System.Collections.Generic;
using Newtonsoft.Json;

namespace sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models
{
    public class TestUserApiKeyJsonEntity : TestModel
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
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// List of API required links
        /// </summary>
        public List<TestEntityLinkModel> Links { get; set; } 
    }
}