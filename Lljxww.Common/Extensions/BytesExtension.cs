using System.Text;
using System.Text.Json;

namespace Lljxww.Common.Extensions
{
    /// <summary>
    /// 字节流扩展方法
    /// </summary>
    public static class BytesExtension
    {
        /// <summary>
        /// 将字节流反序列化为指定的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T? ToObject<T>(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes));
        }
    }
}
