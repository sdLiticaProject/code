using System;
using System.Text;

namespace sdLitica.IntegrationTests.TestUtils
{
    public static class TestStringHelper
    {
        private static string _latinLetters = "abcdefghijklmnopqrstuvwxyz";

        public static string RandomLatinString(int length = 15)
        {
            var result = new StringBuilder();
            var rng = new Random();
            for (int i = 0; i < length; i++)
            {
                result.Append(_latinLetters[rng.Next(_latinLetters.Length)]);
            }

            return result.ToString();
        }
    }
}