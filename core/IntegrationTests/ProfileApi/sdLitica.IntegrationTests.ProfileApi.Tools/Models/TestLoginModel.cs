using System.ComponentModel.DataAnnotations;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tools.Models
{
    public class TestLoginModel : TestModel
    {
        [Display(Name = nameof(Email))] public string Email { get; set; }

        [Display(Name = nameof(Password))] public string Password { get; set; }
    }
}