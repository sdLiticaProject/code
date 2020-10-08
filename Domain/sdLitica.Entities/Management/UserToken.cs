using System;
using System.ComponentModel.DataAnnotations.Schema;
using sdLitica.Entities.Abstractions;
using sdLitica.Utils.Helpers;

namespace sdLitica.Entities.Management
{
    /// <summary>
    /// This class is used to describe UserToken entity
    /// </summary>
    [Table("USER_TOKENS")]
    public class UserToken : Entity
    {
        /// <summary>
        /// User token
        /// </summary>
        [Column("TOKEN")]
        public string Token { get; protected set; }
        /// <summary>
        /// User token expiration date
        /// </summary>
        [Column("EXPIRATION")]
        public DateTime TokenExpirationDate { get; protected set; }
        /// <summary>
        /// User Id identifier
        /// </summary>
        [Column("USER_ID")]
        public Guid UserId { get; protected set; }
        /// <summary>
        /// User Id identifier
        /// </summary>
        public User User { get; protected set; }

        /// <summary>
        /// Creates an UserToken with no parameters
        /// </summary>
        protected UserToken()
        {

        }

        /// <summary>
        /// This method expires the current token in the UserToken object
        /// </summary>
        public void ExpiresToken()
        {
            DateTime now = DateTime.UtcNow;            
            TokenExpirationDate = now.AddDays(-1);
        }

        /// <summary>
        /// This method check if the current token is expired, using current UTC datetime
        /// </summary>
        /// <returns>True if the token is expired</returns>
        public bool IsTokenExpired()
        {
            return DateTime.UtcNow > TokenExpirationDate;
        }

        /// <summary>
        /// This method creates a Token
        /// </summary>
        /// <param name="expiration">Expiration in hours</param>
        public void CreateToken(int expiration)
        {
            DateTime now = DateTime.UtcNow;
            Token = HashHelper.GetSha256(Convert.ToString(now.Ticks));
            TokenExpirationDate = now.AddHours(expiration);
        }

        /// <summary>
        /// This method shifts current token expiration
        /// </summary>
        /// <param name="expiration">Expiration in hours</param>
        public void ShiftCurrentToken(int expiration)
        {
            DateTime now = DateTime.UtcNow;            
            TokenExpirationDate = now.AddHours(expiration);
        }

        /// <summary>
        /// This factory method creates an UserToken to the parameterized user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserToken Create(User user, int expiration)
        {
            UserToken userToken = new UserToken()
            {
                Id = Guid.NewGuid(),
                User = user ?? throw new ArgumentNullException(nameof(user)),
                UserId = user.Id,                
            };

            userToken.CreateToken(expiration);
            return userToken;
        }
    }
}
