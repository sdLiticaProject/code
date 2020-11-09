using System.ComponentModel.DataAnnotations;

namespace sdLitica.WebAPI.Models.Management
{
    /// <summary>
    /// This class represents updated user profile
    /// </summary>
    public class UserUpdateModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = nameof(FirstName))]
        public string FirstName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = nameof(LastName))]
        public string LastName { get; set; }
    }
}
