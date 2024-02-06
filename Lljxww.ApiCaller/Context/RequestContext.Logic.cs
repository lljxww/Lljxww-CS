namespace Lljxww.ApiCaller.Context;

public partial class RequestContext
{
    /// <summary>
    /// 获取自定义对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? GetCustomObject<T>()
    {
        if (CustomObject == null)
        {
            return default;
        }

        try
        {
            return (T)CustomObject;
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// 获取自定义对象, 如果获取失败, 则返回给定的默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T? GetCustomObject<T>(T defaultValue)
    {
        return GetCustomObject<T>() ?? defaultValue;
    }

    /// <summary>
    /// 合并RequestOption中的配置到RequestContext
    /// </summary>
    /// <param name="requestOption"></param>
    /// <returns></returns>
    public void MergeRequestOption(RequestOption requestOption)
    {
        IsTriggerOnExecuted = requestOption.IsTriggerOnExecuted;
        IsFromCache = requestOption.IsFromCache;
        CustomFinalUrlHandler = requestOption.CustomFinalUrlHandler;
        CustomHttpContentHandler = requestOption.CustomHttpContentHandler;
        DontLog = requestOption.DontLog;
        WhenDontSaveRequestCache = requestOption.WhenDontSaveRequestCache;
        CustomAuthorizeInfo = requestOption.CustomAuthorizeInfo;
        Timeout = requestOption.Timeout;
        CustomObject = requestOption.CustomObject;
        CustomCacheKeyPart = requestOption.CustomCacheKeyPart;
    }
}
