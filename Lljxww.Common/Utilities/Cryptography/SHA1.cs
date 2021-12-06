using System;
using System.Text;

namespace Lljxww.Common.Utilities.Cryptography
{
    public class SHA1
    {
        public static string Calculate(string source)
        {
            using System.Security.Cryptography.SHA1? sha = System.Security.Cryptography.SHA1.Create();
            byte[] result = sha.ComputeHash(Encoding.UTF8.GetBytes(source));
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }
    }
}
