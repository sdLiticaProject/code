using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sdLitica.WebAPI.Models.Management
{
    /// <summary>
    /// login model
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = nameof(Name))]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Password))]
        public string Password { get; set; }
    }
}
