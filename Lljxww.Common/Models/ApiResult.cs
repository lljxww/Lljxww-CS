using System;
using System.Collections;
using System.Text.Json;

namespace Lljxww.Common.Models;

/// <summary>
///     实现了IHttpActionResult的返回结果类
/// </summary>
/// <typeparam name="T">返回结果集的类型</typeparam>
[Serializable]
public class ApiResult<T>
{
    /// <summary>
    ///     构造方法
    /// </summary>
    /// <param name="success">是否成功</param>
    /// <param name="message">信息</param>
    /// <param name="code">错误代码</param>
    /// <param name="result">结果集</param>
    /// <param name="count">返回结果数</param>
    /// <param name="total">查找到的结果总数</param>
    public ApiResult(bool success, string? message, int code, T? result = default, int count = 0, int total = 0)
    {
        Count = count;
        Total = total;
        Content = result;
        Message = message;
        Code = code;
        Success = success;

        try
        {
            if (total == 0 && count == 0 && result != null &&
                Array.Exists(typeof(T).GetInterfaces(), t => t == typeof(IEnumerable)))
            {
                Total = ((IList)result).Count;
                Count = Total;
            }
        }
        catch
        {
        }

        if (Content == null && !string.IsNullOrWhiteSpace(message))
        {
            Success = false;
        }
    }

    /// <summary>
    ///     初始化APIResult的新实例
    /// </summary>
    /// <param name="result">Result主体</param>
    /// <param name="message">信息</param>
    public ApiResult(T? result, string? message = null) : this(true, message, 1, result)
    {
    }

    /// <summary>
    ///     初始化APIResult的新实例
    /// </summary>
    /// <param name="success"></param>
    /// <param name="message"></param>
    public ApiResult(bool success, string? message) : this(success, message, 1)
    {
    }

    /// <summary>
    ///     返回记录数
    /// </summary>
    public int Count { get; }

    /// <summary>
    ///     结果/结果集
    /// </summary>
    public T? Content { get; }

    /// <summary>
    ///     提示信息
    /// </summary>
    public string? Message { get; }

    /// <summary>
    ///     错误码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    ///     是否成功
    /// </summary>
    public bool Success { get; }

    /// <summary>
    ///     记录总数
    /// </summary>
    public int Total { get; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}