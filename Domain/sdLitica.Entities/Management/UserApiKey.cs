using System;
using System.ComponentModel.DataAnnotations.Schema;
using sdLitica.Entities.Abstractions;

namespace sdLitica.Entities.Management
{
    /// <summary>
    /// This class represents a single API end-user can create
    /// to access sdLitica API's without exposing credentials
    /// </summary>
    [Table("USER_API_KEYS")]
    public class UserApiKey : Entity
    {
        /// <summary>
        /// Id of a user who owns an API key
        /// </summary>
        [Column("USER_ID")]
        public Guid UserId { get; protected set; }
        
        /// <summary>
        /// API key value
        /// </summary>
        [Column("API_KEY")]
        public string APIKey { get; protected set; }

        /// <summary>
        /// API key value
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; protected set; }

        /// <summary>
        /// User entity to which given token
        /// belongs to
        /// </summary>
        public User User { get; protected set; }

        /// <summary>
        /// Creates an user api key instance with no parameters
        /// </summary>
        protected UserApiKey()
        {

        }

        public static UserApiKey CreateNew(User user, string description)
        {
            UserApiKey userApiKey = new UserApiKey
            {
                Id = Guid.NewGuid(),
                APIKey = UserApiKey.generateApiKey(user),
                UserId = user.Id,
                Description = description
            };


            return userApiKey;
        }

        private static string generateApiKey(User user)
        {
            // TODO: We need to find more smart way of generation API 
            // key like JWT token, so we will be able to validate
            // it without queries to database upon each request
            byte[] bites = System.Text.Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            return System.Convert.ToBase64String(bites);
        }

    }
}
