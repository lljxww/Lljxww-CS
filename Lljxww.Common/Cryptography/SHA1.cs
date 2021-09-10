using System.Security.Cryptography;
using System.Text;

namespace Lljxww.Common.Cryptography
{
    public class SHA1
    {
        public string Get(string source)
        {
            using SHA1CryptoServiceProvider? sha = new();
            byte[]? bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(source));
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
