using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Lljxww.Common.Extensions;

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

    /// <summary>
    /// 将对象中的空值删除, 其余值作为字典的值, 其键作为字典的键生成字典对象
    /// 仅支持属性为基本类型的匿名对象或类
    /// </summary>
    /// <param name="obj">属性为基本类型的对象</param>
    /// <returns></returns>
    public static Dictionary<string, string> ToDictionaryWithoutNullProperties(this object obj)
    {
        Dictionary<string, string> result = new();

        foreach (PropertyInfo? property in obj.GetType().GetProperties())
        {
            string? value = property.GetValue(obj)?.ToString();

            if (!string.IsNullOrWhiteSpace(value))
            {
                result.Add(property.Name, value);
            }
        }

        return result;
    }
}