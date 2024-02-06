using Lljxww.ApiCaller.Config;
using Lljxww.ApiCaller.Context;
using Microsoft.Extensions.Options;

namespace Lljxww.ApiCaller.RequestContextLoader;

public class ConfigRequestContextLoader : IRequestContextLoader
{
    public ConfigRequestContextLoader(IOptions<ApiCallerConfig> options)
    {
        apiCallerConfig = options.Value;
    }

    private ApiCallerConfig apiCallerConfig;

    public RequestContext LoadAsRequestContext(string name, object? param = null, RequestOption? requestOption = null)
    {
        (ServiceItem serviceItem, ApiItem apiItem) = ParseConfig(name, apiCallerConfig);

        RequestContext requestContext = new()
        {
            Environment = CallerOption.Environment.ToString(),
            ApiName = name,
            Description = apiItem.Description,
            AuthorizationName = apiItem.AuthorizationType ?? serviceItem.AuthorizationType,
            Url = serviceItem.BaseUrl + apiItem.Url,
            HttpMethod = apiItem.HttpMethod,
            Param = param,
            ParamType = apiItem.ParamType.ToLower() switch
            {
                "query" => ParamPosition.QueryString,
                "body" => ParamPosition.Body,
                "path" => ParamPosition.Path,
                "header" => ParamPosition.Header,
                _ => ParamPosition.None
            },
            ContentType = apiItem.ContentType,
            Timeout = apiItem.Timeout,
            NeedCache = apiItem.NeedCache,
            CacheTime = apiItem.CacheTime,
            EncodeUrl = apiItem.EncodeUrl,
            IsTriggerOnExecuted = requestOption?.IsTriggerOnExecuted ?? true,
            IsFromCache = requestOption?.IsFromCache ?? true,
            CustomFinalUrlHandler = requestOption?.CustomFinalUrlHandler,
            CustomHttpContentHandler = requestOption?.CustomHttpContentHandler,
            DontLog = requestOption?.DontLog ?? false,
            WhenDontSaveRequestCache = requestOption?.WhenDontSaveRequestCache ?? (_ => false),
            CustomAuthorizeInfo = requestOption?.CustomAuthorizeInfo,
            CustomCacheKeyPart = requestOption?.CustomCacheKeyPart,
            CustomObject = requestOption?.CustomObject
        };

        return requestContext;
    }

    /// <summary>
    /// 找出给定的目标服务名的服务节点和接口节点配置
    /// </summary>
    /// <param name="apiNameAndMethodName">服务名和接口名</param>
    /// <param name="config">接口调用配置</param>
    /// <returns>服务节点，接口节点</returns>
    /// <exception cref="ConfigurationErrorsException"></exception>
    private static (ServiceItem, ApiItem) ParseConfig(string apiNameAndMethodName, ApiCallerConfig config)
    {
        string serviceName = apiNameAndMethodName.Split('.')[0];
        string methodName = apiNameAndMethodName.Split('.')[1];

        if (config.ServiceItems.All(i => i.ApiName.ToLower().Trim() != serviceName.ToLower().Trim()))
        {
            throw new Exception($"未找到指定的方法: {serviceName}");
        }

        ServiceItem serviceItem = config.ServiceItems
            .Single(a => a.ApiName.ToLower().Trim() == serviceName.ToLower().Trim());
        ApiItem apiItem = serviceItem.ApiItems
            .Single(c => c.Method.ToLower().Trim() == methodName.ToLower().Trim());

        return (serviceItem, apiItem);
    }
}
