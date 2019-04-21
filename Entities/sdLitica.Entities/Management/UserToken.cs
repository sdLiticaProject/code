
using sdLitica.Entities.Abstractions;
using System;

namespace sdLitica.Entities.Management
{
    /// <summary>
    /// This class used to describe UserToken entity in database
    /// </summary>
    public class UserToken : Entity
    {
        /// <summary>
        /// User token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// User token expiration date
        /// </summary>
        public DateTime TokenExpirationDate { get; set; }
        /// <summary>
        /// User Id identifier
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// User Id identifier
        /// </summary>
        public User User { get; set; }
    }
}
