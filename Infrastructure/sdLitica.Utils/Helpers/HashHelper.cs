using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace sdLitica.Utils.Helpers
{
    public static class HashHelper
    {
        public static string GetSha256(string data)
        {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(data);
            var hash = sha256.ComputeHash(bytes);
            sha256.Dispose();

            var builder = new StringBuilder();
            foreach(var b in hash) builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}
