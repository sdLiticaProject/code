using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdLitica.IntegrationTests.TestUtils
{
	public static class TestStringHelper
	{
		private static string _latinLetters = "abcdefghijklmnopqrstuvwxyz";
		private static string _cyrillicLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
		private static string _numbers = "0123456789";
		private static string _specCharacters = "!@$%^&*(){}[]\\\"'/?:;<>,.`~";

		public static string RandomStringFromCollection(int length, IReadOnlyCollection<char> chars)
		{
			var result = new StringBuilder();
			var rng = new Random();
			var charsArray = chars.ToArray();
			for (int i = 0; i < length; i++)
			{
				result.Append(charsArray[rng.Next(charsArray.Length)]);
			}

			return result.ToString();
		}


		public static string RandomLatinString(int length = 15)
		{
			return RandomStringFromCollection(length, _latinLetters.ToCharArray());
		}

		public static string RandomCyrillicString(int length = 15)
		{
			return RandomStringFromCollection(length, _cyrillicLetters.ToCharArray());
		}

		public static string RandomNumberString(int length = 15)
		{
			return RandomStringFromCollection(length, _numbers.ToCharArray());
		}

		public static string RandomSpecCharactersString(int length = 15)
		{
			return RandomStringFromCollection(length, _specCharacters.ToCharArray());
		}

		public static string RandomMixedString(int length = 15)
		{
			return RandomStringFromCollection(length,
				(_numbers + _specCharacters + _cyrillicLetters + _latinLetters).ToCharArray());
		}
	}
}
