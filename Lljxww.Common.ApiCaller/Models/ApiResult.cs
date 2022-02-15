using System.Text.Json;
using System.Text.Json.Nodes;

namespace Lljxww.Common.ApiCaller.Models
{
    /// <summary>
    /// ApiCaller接口返回结果包装类
    /// </summary>
    [Serializable]
    public class ApiResult
    {
        private bool _isSet = false;
        private bool _success = false;

        /// <summary>
        /// 执行结果(试用,实际情况以RawStr自行判断)
        /// </summary>
        public bool Success
        {
            get
            {
                if (_isSet)
                {
                    return _success;
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
                _success = value;
                _isSet = true;
            }
        }

        private string _rawStr;

        /// <summary>
        /// 接口的原始返回结果
        /// </summary>
        public string RawStr
        {
            get => _rawStr;
            set
            {
                try
                {
                    JsonObject = JsonNode.Parse(value)?.AsObject();
                }
                catch
                {
                    JsonObject = new JsonObject();
                }
                _rawStr = value;
            }
        }

        private string? _message;

        /// <summary>
        /// 执行信息
        /// </summary>
        public string? Message
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_message))
                {
                    return _message;
                }

                try
                {
                    return this[nameof(Message)];
                }
                catch
                {
                    return "";
                }
            }
            set => _message = value;
        }

        public ApiResult(string resultStr)
        {
            RawStr = resultStr;

            try
            {
                JsonObject = JsonNode.Parse(RawStr)?.AsObject();
            }
            catch
            {
                JsonObject = new JsonObject();
            }
        }

        public ApiResult(bool success, string message)
        {
            _success = success;
            _message = message;
        }

        public ApiResult()
        {
            try
            {
                JsonObject = !string.IsNullOrWhiteSpace(RawStr) ? JsonNode.Parse(RawStr)?.AsObject() : new JsonObject();
            }
            catch
            {
                JsonObject = new JsonObject();
            }
        }

        [NonSerialized]
        public JsonObject? JsonObject;

        /// <summary>
        /// 返回结果索引器
        /// </summary>
        /// <param name="propertyName">键名</param>
        /// <returns>返回值</returns>
        public string? this[string propertyName]
        {
            get
            {
                try
                {
                    JsonNode? result = default;
                    bool? success = JsonObject?.TryGetPropertyValue(propertyName, out result);

                    if (success.HasValue && success.Value)
                    {
                        return result?.GetValue<string>();
                    }

                    return string.Empty;
                }
                catch (NullReferenceException)
                {
                    JsonObject = JsonNode.Parse(_rawStr)?.AsObject();
                    try
                    {
                        JsonNode? result = default;
                        bool? success = JsonObject?.TryGetPropertyValue(propertyName, out result);

                        if (success.HasValue && success.Value)
                        {
                            return result?.GetValue<string>();
                        }

                        return string.Empty;
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
                T? instance = JsonSerializer.Deserialize<T>(RawStr);
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
            return new ApiResult(JsonSerializer.Serialize(obj));
        }
    }
}
