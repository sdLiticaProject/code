using System;
using System.Text;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;

namespace sdLitica.IntegrationTests.ProfileApi.Tools.Helpers
{
    public class ProfileHelper
    {
        public static string CompareUserProfiles(TestUserModel expected, TestUserModel actual)
        {
            var result = new StringBuilder();
            if (!String.Equals(expected.Email, actual.Email))
            {
                result.Append($"Users have different emails: '{expected.Email}' != '{actual.Email}'\n");
            }

            if (!String.Equals(expected.FirstName, actual.FirstName))
            {
                result.Append($"Users have different first names: '{expected.FirstName}' != '{actual.FirstName}'\n");
            }

            if (!String.Equals(expected.LastName, actual.LastName))
            {
                result.Append($"Users have different last names: '{expected.LastName}' != '{actual.LastName}'\n");
            }

            if (actual.Id == null || Equals(actual.Id, String.Empty))
            {
                result.Append("Actual profile should have id, but it was null or empty\n");
            }

            if (actual.Password != null)
            {
                result.Append($"Actual profile should have null password, but it was '{actual.Password}'\n");
            }

            return result.ToString();
        }
    }
}