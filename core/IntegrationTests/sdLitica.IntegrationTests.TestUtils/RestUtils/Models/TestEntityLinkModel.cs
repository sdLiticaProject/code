namespace sdLitica.IntegrationTests.TestUtils.RestUtils.Models
{
    public class TestEntityLinkModel
    {
        /// <summary>
        /// Type of the link to be created
        /// </summary>
        public enum TestLinkType
        {
            /// <summary>
            /// Link type self
            /// </summary>
            Self,
            /// <summary>
            /// Link type canonical
            /// </summary>
            Canonical
        }

        /// <summary>
        /// Type of the link, i.e. "self" or "canonical"
        /// </summary>

        public string Type { get; set; }

        /// <summary>
        /// Link address
        /// </summary>
        public string Href { get; set; }
    }
}
