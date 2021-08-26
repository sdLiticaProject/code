namespace sdLitica.IntegrationTests.ProfileApi.Tools.Models
{
    public class TestEntityLinkModel
    {
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