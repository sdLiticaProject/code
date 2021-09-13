using System.ComponentModel.DataAnnotations;

namespace sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models
{
    public class TestUserUpdateModel : TestModel
    {
        [Display(Name = nameof(FirstName))] public string FirstName { get; set; }

        [Display(Name = nameof(LastName))] public string LastName { get; set; }
    }
}