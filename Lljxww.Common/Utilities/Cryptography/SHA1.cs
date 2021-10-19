using System;
using System.Security.Cryptography;
using System.Text;

namespace Lljxww.Common.Utilities.Cryptography
{
    public class SHA1
    {
        public static string Calculate(string source)
        {
            using SHA1CryptoServiceProvider? sha = new();
            byte[] result = sha.ComputeHash(Encoding.UTF8.GetBytes(source));
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }
    }
}
