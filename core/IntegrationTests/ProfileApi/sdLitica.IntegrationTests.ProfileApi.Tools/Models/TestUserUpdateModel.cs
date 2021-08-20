using System.ComponentModel.DataAnnotations;
using sdLitica.IntegrationTests.TestUtils;

namespace sdLitica.IntegrationTests.ProfileApi.Tools.Models
{
    public class TestUserUpdateModel : TestModel
    {
        [Display(Name = nameof(FirstName))] public string FirstName { get; set; }

        [Display(Name = nameof(LastName))] public string LastName { get; set; }
    }
}