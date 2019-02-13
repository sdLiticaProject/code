
namespace sdLitica.Entities.Management
{
    /// <summary>
    /// this class used to describe ProfileTokens entity in tarantool database
    /// </summary>
    public class ProfileToken
    {
        /// <summary>
        /// Profile token
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// Profile token expiration date
        /// </summary>
        public long TokenExpirationDate { get; set; }

        /// <summary>
        /// Profile id identifier
        /// </summary>
        public string ProfileId { get; set; }
    }
}
