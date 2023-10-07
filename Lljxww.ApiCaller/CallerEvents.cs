using Lljxww.ApiCaller.Models;

namespace Lljxww.ApiCaller;

public static class CallerEvents
{
    /// <summary>
    /// 读取缓存
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public delegate ApiResult? GetCacheHandler(CallerContext context);

    /// <summary>
    /// 记录日志
    /// </summary>
    /// <param name="context"></param>
    public delegate void LogHandler(CallerContext context);

    /// <summary>
    /// 执行发生异常时触发
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ex"></param>
    public delegate void OnExceptionHandler(CallerContext context, Exception ex);

    /// <summary>
    /// 请求方法执行结束后的操作
    /// </summary>
    /// <param name="context"></param>
    public delegate void OnExecutedHandler(CallerContext context);

    /// <summary>
    /// 请求超时时触发
    /// </summary>
    /// <param name="context"></param>
    public delegate void OnRequestTimeoutHandler(CallerContext context);

    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public delegate void SetCacheHandler(CallerContext context);

    public static event SetCacheHandler SetCacheEvent;

    internal static void SetCache(CallerContext context)
    {
        if (SetCacheEvent == null)
        {
            return;
        }

        try
        {
            _ = Task.Run(() => SetCacheEvent.Invoke(context));
        }
        catch
        {
            // ignored
        }
    }

    public static event GetCacheHandler GetCacheEvent;

    internal static ApiResult? GetCache(CallerContext context)
    {
        if (GetCacheEvent == null)
        {
            return null;
        }

        try
        {
            return GetCacheEvent.Invoke(context);
        }
        catch
        {
            return null;
        }
    }

    public static event LogHandler LogEvent;

    internal static void Log(CallerContext context)
    {
        if (LogEvent == null)
        {
            return;
        }

        try
        {
            _ = Task.Run(() => LogEvent(context));
        }
        catch
        {
            // ignored
        }
    }

    public static event OnExecutedHandler OnExecuted;

    internal static void Executed(CallerContext context)
    {
        if (OnExecuted == null)
        {
            return;
        }

        try
        {
            _ = Task.Run(() => OnExecuted.Invoke(context));
        }
        catch
        {
            // ignored
        }
    }

    public static event OnExceptionHandler OnException;

    internal static void Exception(CallerContext context, Exception ex)
    {
        if (OnException == null)
        {
            return;
        }

        try
        {
            _ = Task.Run(() => OnException.Invoke(context, ex));
        }
        catch
        {
            // ignored
        }
    }

    public static event OnRequestTimeoutHandler OnRequestTimeout;

    internal static void RequestTimeout(CallerContext context)
    {
        if (OnRequestTimeout == null)
        {
            return;
        }

        try
        {
            _ = Task.Run(() => OnRequestTimeout.Invoke(context));
        }
        catch
        {
            // ignored
        }
    }
}