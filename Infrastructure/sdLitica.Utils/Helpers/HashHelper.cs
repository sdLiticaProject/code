using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace sdLitica.Utils.Helpers
{
    /// <summary>
    /// Helper class to any Hash processing
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// This method computes a SHA256 hash (in Hex) for the data parameterized
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSha256(string data)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = sha256.ComputeHash(bytes);
            sha256.Dispose();

            StringBuilder builder = new StringBuilder();
            foreach(var b in hash) builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}
