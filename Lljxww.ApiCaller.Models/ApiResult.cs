using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Lljxww.ApiCaller.Models;

/// <summary>
/// ApiCaller接口返回结果包装类
/// </summary>
[Serializable]
public class ApiResult
{
    [IgnoreDataMember] public HttpResponseMessage? HttpResponseMessage { get; private set; }

    #region Success

    public delegate bool GetSuccessHandler(ApiResult apiResult, Predicate<ApiResult> defaultPredicate);

    public static event GetSuccessHandler GetSuccess;

    private static bool GetSuccessDefault(ApiResult apiResult)
    {
        try
        {
            return apiResult.JsonObject![nameof(Success)]!.GetValue<bool>();
        }
        catch
        {
            try
            {
                return apiResult.JsonObject!["IsSuccess"]!.GetValue<bool>();
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 执行结果(如默认方法无法正常工作，可通过GetSuccess事件自行注册)
    /// </summary>
    public bool Success => GetSuccess != null ? GetSuccess(this, GetSuccessDefault) : GetSuccessDefault(this);

    #endregion

    #region Code

    public delegate int GetCodeHandler(ApiResult apiResult, Func<ApiResult, int> defaultFunc);

    public static event GetCodeHandler GetCode;

    private static int GetCodeDefault(ApiResult apiResult)
    {
        try
        {
            return apiResult.JsonObject!["code"]!.GetValue<int>();
        }
        catch
        {
            return -1;
        }
    }

    /// <summary>
    /// 结果中的Code(如默认方法无法正常工作，可通过GetCode事件自行注册)
    /// </summary>
    public int Code => GetCode != null ? GetCode(this, GetCodeDefault) : GetCodeDefault(this);

    #endregion

    #region Message

    public delegate string GetMessageHandler(ApiResult apiResult, Func<ApiResult, string> defaultFunc);

    public static event GetMessageHandler GetMessage;

    private static string? GetMessageDefault(ApiResult apiResult)
    {
        try
        {
            return apiResult[nameof(Message)];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 执行信息(如默认方法无法正常工作，可通过GetMessage事件自行注册)
    /// </summary>
    public string? Message => GetMessage != null ? GetMessage(this, GetMessageDefault) : GetMessageDefault(this);

    #endregion

    #region RawString

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
                JsonObject = JsonNode.Parse(value, _jnOption)?.AsObject() ?? new JsonObject(_jnOption);
            }
            catch
            {
                JsonObject = new JsonObject(_jnOption);
            }

            _rawStr = value;
        }
    }

    #endregion

    #region Ctor

    public ApiResult(string resultStr, HttpResponseMessage? response = null, CallerContext? context = null)
    {
        RawStr = resultStr;
        HttpResponseMessage = response;

        try
        {
            JsonObject = JsonNode.Parse(RawStr, _jnOption)?.AsObject() ?? new JsonObject(_jnOption);
        }
        catch
        {
            JsonObject = new JsonObject(_jnOption);
        }
    }

    public ApiResult(object result, HttpResponseMessage? response = null, CallerContext? context = null)
    {
        try
        {
            JsonObject = JsonSerializer.SerializeToNode(result, _jsOption)?.AsObject() ?? new JsonObject(_jnOption);
        }
        catch
        {
            JsonObject = new JsonObject(_jnOption);
        }

        HttpResponseMessage = response;
    }

    public ApiResult()
    {
    }

    #endregion

    #region PublicMethod

    [NonSerialized] public JsonObject JsonObject;

    [NonSerialized] public CallerContext? Context;

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
                JsonNode? result = default;
                bool? success = JsonObject?.TryGetPropertyValue(propertyName, out result);

                return !success.HasValue || !success.Value ? null : (result?.ToJsonString(_jsOption));
            }
            catch (NullReferenceException)
            {
                JsonObject = JsonNode.Parse(_rawStr, _jnOption)?.AsObject() ?? new JsonObject(_jnOption);
                try
                {
                    JsonNode? result = default;
                    bool? success = JsonObject?.TryGetPropertyValue(propertyName, out result);

                    return success.HasValue && success.Value ? (result?.ToJsonString(_jsOption)) : null;
                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                return null;
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

    #endregion

    #region Options

    private JsonNodeOptions _jnOption = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly JsonSerializerOptions _jsOption = new()
    {
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };

    #endregion
}