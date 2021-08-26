﻿using System.Collections.Generic;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tools.Models
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
        public string Description { get; set; }

        /// <summary>
        /// List of API required links
        /// </summary>
        public List<TestEntityLinkModel> Links { get; set; } 
    }
}