using System.ComponentModel.DataAnnotations;

namespace sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models
{
    public class TestLoginModel : TestModel
    {
        [Display(Name = nameof(Email))] public string Email { get; set; }

        [Display(Name = nameof(Password))] public string Password { get; set; }
    }
}