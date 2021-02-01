using System.ComponentModel.DataAnnotations.Schema;
using sdLitica.Entities.Abstractions;

namespace sdLitica.Entities.Management
{
    /// <summary>
    /// This class represents a single API end-user can create
    /// to access sdLitica API's without exposing credentials
    /// </summary>
    [Table("USER_API_KEYS")]
    public class UserAPIKey : Entity
    {
        /// <summary>
        /// Id of a user who owns an API key
        /// </summary>
        [Column("USER_ID")]
        public string UserId { get; protected set; }
        
        /// <summary>
        /// API key value
        /// </summary>
        [Column("API_KEY")]
        public string APIKey { get; protected set; }

        /// <summary>
        /// User entity to which given token
        /// belongs to
        /// </summary>
        public User User { get; protected set; }

        /// <summary>
        /// Creates an user api key instance with no parameters
        /// </summary>
        protected UserAPIKey()
        {

        }

    }
}
