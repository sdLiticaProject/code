
namespace sdLitica.WebAPI.Models.Management
{
    /// <summary>
    /// This class represents unique identity and info of the user
    /// </summary>
    public class CreateUserModel
    {
        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Creates an user with no parameters
        /// </summary>
        public CreateUserModel()
        {

        }
    }
}