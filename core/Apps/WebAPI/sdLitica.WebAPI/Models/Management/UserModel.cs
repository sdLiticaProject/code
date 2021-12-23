
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace sdLitica.WebAPI.Models.Management
{
    /// <summary>
    /// This class represents unique identity and info of the user
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Unique user identifier, read-only
        /// </summary>
        [MaxLength(36)]
        public string Id { get; set; }
        /// <summary>
        /// User first name
        /// </summary>
        [MaxLength(45)]
        public string FirstName { get; set; }
        /// <summary>
        /// User last name
        /// </summary>
        [MaxLength(45)]
        public string LastName { get; set; }
        /// <summary>
        /// User email
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(45)]
        public string Email { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [Required]
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>
        /// Creates an user with no parameters
        /// </summary>
        public UserModel()
        {

        }

        /// <summary>
        /// Creates an user from internal user model
        /// </summary>
        public UserModel(sdLitica.Entities.Management.User user)
        {
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Password = null;
            this.Id = user.Id.ToString();
        }

    }
}
