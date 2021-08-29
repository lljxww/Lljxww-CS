using Newtonsoft.Json.Linq;
using System;

namespace Lljxww.Common.WebApiCaller.Models
{
    /// <summary>
    /// ApiCaller接口返回结果包装类
    /// </summary>
    [Serializable]
    public class ApiResult
    {
        private bool isSet = false;
        private bool success = false;

        /// <summary>
        /// 执行结果(试用,实际情况以RawStr自行判断)
        /// </summary>
        public bool Success
        {
            get
            {
                if (isSet)
                {
                    return success;
                }
                else
                {
                    try
                    {
                        return Convert.ToBoolean(this[nameof(Success)]);
                    }
                    catch
                    {
                        try
                        {
                            return Convert.ToBoolean(this["IsSuccess"]);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            set
            {
                success = value;
                isSet = true;
            }
        }

        private string rawStr;

        /// <summary>
        /// 接口的原始返回结果
        /// </summary>
        public string RawStr
        {
            get => rawStr;
            set
            {
                try
                {
                    JsonObject = JObject.Parse(value);
                }
                catch
                {
                    JsonObject = new JObject();
                }
                rawStr = value;
            }
        }

        private string message;

        /// <summary>
        /// 执行信息(试用,实际情况以RawStr自行判断)
        /// </summary>
        public string Message
        {
            get
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    try
                    {
                        return this[nameof(Message)].ToString();
                    }
                    catch
                    {
                        return "";
                    }
                }
                else
                {
                    return message;
                }
            }
            set => message = value;
        }

        public ApiResult(string resultStr)
        {
            RawStr = resultStr;

            try
            {
                JsonObject = JObject.Parse(RawStr);
            }
            catch
            {
                JsonObject = new JObject();
            }
        }

        public ApiResult(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }

        public ApiResult()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(RawStr))
                {
                    JsonObject = JObject.Parse(RawStr);
                }
                else
                {
                    JsonObject = new JObject();
                }
            }
            catch
            {
                JsonObject = new JObject();
            }
        }

        [NonSerialized]
        public JObject JsonObject;

        /// <summary>
        /// 返回结果索引器
        /// </summary>
        /// <param name="propertyName">键名</param>
        /// <returns>返回值</returns>
        public string this[string propertyName]
        {
            get
            {
                try
                {
                    JsonObject.TryGetValue(propertyName, StringComparison.OrdinalIgnoreCase, out JToken result);
                    return result != null ? result.ToString() : string.Empty;
                }
                catch (NullReferenceException)
                {
                    JsonObject = JObject.Parse(rawStr);
                    try
                    {
                        JsonObject.TryGetValue(propertyName, StringComparison.OrdinalIgnoreCase, out JToken result);
                        return result != null ? result.ToString() : string.Empty;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 将Result的原始字符串反序列化为指定的格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? TryConvert<T>(T? defaultValue = default)
        {
            try
            {
                T? instance = System.Text.Json.JsonSerializer.Deserialize<T>(RawStr);
                return instance;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 使用对象构建ApiResult实例
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ApiResult Build(object obj)
        {
            return new ApiResult(System.Text.Json.JsonSerializer.Serialize(obj));
        }
    }
}
