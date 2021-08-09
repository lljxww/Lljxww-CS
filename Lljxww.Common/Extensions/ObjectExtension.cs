using System.Text;
using System.Text.Json;

namespace Lljxww.Common.Extensions
{
    /// <summary>
    /// object扩展类
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 将对象序列化为字节流
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this object obj)
        {
            string jsonStr = JsonSerializer.Serialize(obj);
            return Encoding.UTF8.GetBytes(jsonStr);
        }
    }
}
