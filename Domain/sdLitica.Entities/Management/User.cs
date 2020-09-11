using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using sdLitica.Entities.Abstractions;
using sdLitica.Utils.Helpers;

namespace sdLitica.Entities.Management
{
    /// <summary>
    /// This class represents unique identity and info of the user
    /// </summary>
    [Table("USERS")]
    public class User : Entity
    {
        /// <summary>
        /// User first name
        /// </summary>
        [Column("FIRST_NAME")]
        public string FirstName { get; protected set; }
        /// <summary>
        /// User last name
        /// </summary>
        [Column("LAST_NAME")]
        public string LastName { get; protected set; }
        /// <summary>
        /// User email
        /// </summary>
        [Column("EMAIL")]
        public string Email { get; protected set; }
        /// <summary>
        /// User password
        /// </summary>
        [Column("PASSWORD")]
        public string Password { get; protected set; }

        /// <summary>
        /// Creates an user with no parameters
        /// </summary>
        protected User()
        {

        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="plainPassword">Raw and plain password</param>
        public void ChangePassword(string plainPassword)
        {
            Password = Encrypt(plainPassword);
        }

        /// <summary>
        /// This method verifies if plain password matches with User password
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <returns>True if it matches successfully</returns>
        public bool MatchPassword(string plainPassword)
        {
            return Password.Equals(Encrypt(plainPassword));
        }

        private string Encrypt(string plainPassword)
        {
            return HashHelper.GetSha256(plainPassword);
        }

        /// <summary>
        /// This factory method creates an user with required parameters
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="plainPassword"></param>
        /// <returns>Returns an user with already encrypted password</returns>
        public static User Create(string firstName, string lastName, string email, string plainPassword)
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email                
            };
            user.ChangePassword(plainPassword);
            return user;
        }
    }
}
