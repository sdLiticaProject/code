namespace sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models
{
    public class TestUserModel : TestModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public TestUserModel ApplyUpdate(TestUserUpdateModel updateModel)
        {
            FirstName = updateModel.FirstName;
            LastName = updateModel.LastName;
            return this;
        }
    }
}