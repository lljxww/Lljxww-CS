using Lljxww.ApiCaller.Exceptions;
using Lljxww.ApiCaller.Extensions;
using Lljxww.ApiCaller.Models;
using Lljxww.ApiCaller.Models.Config;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Lljxww.ApiCaller;

public class CallerContext
{
    private static readonly Dictionary<string, Func<CallerContext, CallerContext>> AuthorizateFuncs = new();

    private CallerContext()
    {
    }

    /// <summary>
    /// 注册授权操作
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="func">操作Func(CallerContext, AuthResult)</param>
    public static void AddAuthFunc(string key, Func<CallerContext, CallerContext> func)
    {
        AuthorizateFuncs.Add(key, func);
    }

    /// <summary>
    /// 编辑请求体。若在RequestOption中设置了自定义请求，则此处不生效
    /// </summary>
    /// <param name="modifyParamFunc"></param>
    public void ModifyRequestBody(Func<object, object> modifyParamFunc)
    {
        if (ApiItem.ParamType != "body")
        {
            return;
        }

        if (OriginParam == null && RequestOption?.CustomHttpContent == null)
        {
            return;
        }

        if (RequestOption?.CustomHttpContent != null)
        {
            return;
        }

        OriginParam = modifyParamFunc.Invoke(OriginParam);
        ParamDic = OriginParam.AsDictionary();

        if (!string.IsNullOrWhiteSpace(ApiItem.ContentType))
        {
            HttpContent = new StringContent(JsonSerializer.Serialize(OriginParam));
            HttpContent.Headers.ContentType = new MediaTypeHeaderValue(ApiItem.ContentType);
        }
        else
        {
            if (ParamDic != null)
            {
                HttpContent = new FormUrlEncodedContent(ParamDic!);
            }
        }

        RequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod,
            RequestUri = new Uri(FinalUrl),
            Content = HttpContent
        };
    }

    /// <summary>
    // 创建Caller上下文实例
    /// </summary>
    /// <param name="apiNameAndMethodName">服务名.方法名</param>
    /// <param name="config">配置对象</param>
    /// <param name="param">参数对象</param>
    /// <param name="requestOption"></param>
    /// <returns></returns>
    internal static CallerContext Build(string apiNameAndMethodName,
        ApiCallerConfig config,
        object? param,
        RequestOption requestOption)
    {
        (ServiceItem serviceItem, ApiItem apiItem) = ParseConfig(apiNameAndMethodName, config);

        string authName = string.IsNullOrWhiteSpace(apiItem.AuthorizationType)
            ? serviceItem.AuthorizationType
            : apiItem.AuthorizationType;

        string? authInfo = "";
        if (!string.IsNullOrWhiteSpace(authName))
        {
            authInfo = !string.IsNullOrWhiteSpace(requestOption.CustomAuthorizeInfo)
                ? requestOption.CustomAuthorizeInfo
                : config.Authorizations.FirstOrDefault(a => a.Name == authName)?.AuthorizationInfo;
        }

        CallerContext context = new()
        {
            Config = config,
            ApiName = apiNameAndMethodName,
            OriginParam = param,
            ParamDic = param?.AsDictionary(),
            ServiceItem = serviceItem,
            BaseUrl = serviceItem.BaseUrl,
            ApiItem = apiItem,
            HttpMethod = new HttpMethod(apiItem.HttpMethod),
            NeedCache = apiItem.NeedCache,
            CacheMinuties = apiItem.CacheTime,
            RequestOption = requestOption,
            Timeout = requestOption.Timeout != 0
                ? requestOption.Timeout
                : apiItem.Timeout != 0
                    ? apiItem.Timeout
                    : serviceItem.Timeout != 0
                        ? serviceItem.Timeout
                        : 0,
            Authorization = new Authorization
            {
                AuthorizationInfo = authInfo,
                Name = authName
            }
        };

        context = ConfigureCache(context);
        context = ConfigureRequestParam(context);
        context = ConfigureAuth(config, context);

        return context;
    }

    internal async Task<CallerContext> RequestAsync()
    {
        ResultFrom = "Request";

        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.CancelAfter(Timeout);

        Stopwatch sw = new();

        try
        {
            HttpClient client = HttpClientInstance.Get(this);

            sw.Start();

            HttpResponseMessage response = client.SendAsync(RequestMessage, cancellationTokenSource.Token).Result;
            ResponseContent = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(ResponseContent))
            {
                ApiResult = new ApiResult(ResponseContent, response, this);
            }
        }
        finally
        {
            sw.Stop();
            Runtime = Convert.ToInt32(sw.ElapsedMilliseconds);
        }

        return this;
    }

    #region Private Configure Actions

    private static (ServiceItem, ApiItem) ParseConfig(string apiNameAndMethodName, ApiCallerConfig config)
    {
        string serviceName = apiNameAndMethodName.Split('.')[0];
        string methodName = apiNameAndMethodName.Split('.')[1];

        if (config.ServiceItems.All(i => i.ApiName.ToLower().Trim() != serviceName.ToLower().Trim()))
        {
            throw new ConfigurationErrorsException($"未找到指定的方法: {serviceName}");
        }

        ServiceItem serviceItem = config.ServiceItems
            .Single(a => a.ApiName.ToLower().Trim() == serviceName.ToLower().Trim());
        ApiItem apiItem = serviceItem.ApiItems
            .Single(c => c.Method.ToLower().Trim() == methodName.ToLower().Trim());

        return (serviceItem, apiItem);
    }

    private static CallerContext ConfigureAuth(ApiCallerConfig config, CallerContext context)
    {
        // 授权
        if (!string.IsNullOrWhiteSpace(context.ServiceItem.AuthorizationType))
        {
            context.Authorization = config.Authorizations.Single(a =>
                a.Name.ToLower().Trim() == context.ServiceItem.AuthorizationType.ToLower().Trim());

            if (context.Authorization == null)
            {
                throw new CallerException($"找不到授权配置：{context.ServiceItem.AuthorizationType}");
            }
        }

        if (!string.IsNullOrWhiteSpace(context.ApiItem.AuthorizationType))
        {
            context.Authorization = config.Authorizations.Single(a =>
                a.Name.ToLower().Trim() == context.ApiItem.AuthorizationType.ToLower().Trim());

            if (context.Authorization == null)
            {
                throw new CallerException($"找不到授权配置：{context.ServiceItem.AuthorizationType}");
            }
        }

        // 添加自定义AuthorizeInfo
        if (string.IsNullOrWhiteSpace(context.RequestOption.CustomAuthorizeInfo))
        {
            return context;
        }

        if (context.Authorization == null)
        {
            throw new CallerException($"找不到授权配置：{context.ServiceItem.AuthorizationType}");
        }

        context.Authorization.AuthorizationInfo = context.RequestOption.CustomAuthorizeInfo;

        if (context.Authorization == null || string.IsNullOrWhiteSpace(context.Authorization?.Name))
        {
            return context;
        }

        if (!AuthorizateFuncs.ContainsKey(context.Authorization!.Name))
        {
            throw new CallerException($"未配置给定的授权方式: {context.Authorization!.Name}");
        }

        context = AuthorizateFuncs[context.Authorization!.Name].Invoke(context);

        return context;
    }

    private static CallerContext ConfigureRequestParam(CallerContext context)
    {
        // 请求地址和请求头
        context.FinalUrl = $"{context.BaseUrl.TrimEnd('/')}/{context.ApiItem.Url.TrimStart('/')}";

        // 用户自定义的url
        if (context.RequestOption?.CustomFinalUrlHandler != null)
        {
            context.FinalUrl = context.RequestOption.CustomFinalUrlHandler.Invoke(context.FinalUrl);
        }

        context.RequestMessage = new HttpRequestMessage
        {
            Method = context.HttpMethod,
            RequestUri = new Uri(context.FinalUrl),
            Content = context.HttpContent
        };

        switch (context.ApiItem.ParamType)
        {
            case "query":
                {
                    if (context.ParamDic?.Count > 0)
                    {
                        if (!context.FinalUrl.Contains('?'))
                        {
                            context.FinalUrl += "?";
                        }

                        foreach (KeyValuePair<string, string> keyValuePair in context.ParamDic)
                        {
                            context.FinalUrl += $"&{keyValuePair.Key}={HttpUtility.UrlEncode(keyValuePair.Value)}";
                        }

                        context.FinalUrl = context.FinalUrl.Replace("?&", "?");
                    }

                    break;
                }
            case "path":
                {
                    if (context.ParamDic != null)
                    {
                        foreach (KeyValuePair<string, string> keyValuePair in context.ParamDic)
                        {
                            context.FinalUrl = context.FinalUrl.Replace($"{{{keyValuePair.Key}}}", keyValuePair.Value);
                        }
                    }

                    break;
                }
            case "body":
                {
                    if (context.OriginParam == null && context.RequestOption?.CustomHttpContent == null)
                    {
                        break;
                    }

                    if (context.RequestOption?.CustomHttpContent != null)
                    {
                        context.HttpContent = context.RequestOption.CustomHttpContent;
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(context.ApiItem.ContentType))
                    {
                        context.HttpContent = new StringContent(JsonSerializer.Serialize(context.OriginParam));
                        context.HttpContent.Headers.ContentType = new MediaTypeHeaderValue(context.ApiItem.ContentType);
                    }
                    else
                    {
                        if (context.ParamDic != null)
                        {
                            context.HttpContent = new FormUrlEncodedContent(context.ParamDic!);
                        }
                    }

                    break;
                }
        }

        return context;
    }

    private static CallerContext ConfigureCache(CallerContext context)
    {
        if (!context.NeedCache)
        {
            return context;
        }

        string keySource =
            $"{context.ApiName}+{(context.OriginParam == null ? "" : JsonSerializer.Serialize(context.OriginParam))}"
                .ToLower();

        using SHA1 sha = SHA1.Create();
        byte[] result = sha.ComputeHash(Encoding.UTF8.GetBytes(keySource));
        context.CacheKey = $"caller:{BitConverter.ToString(result).Replace("-", "").ToLower()}";

        return context;
    }

    #endregion

    #region 属性

    /// <summary>
    /// Caller全局设置
    /// </summary>
    public ApiCallerConfig Config { get; private set; }

    /// <summary>
    /// 服务名.方法名
    /// </summary>
    public string ApiName { get; private set; }

    public HttpMethod HttpMethod { get; private set; }

    public HttpRequestMessage RequestMessage { get; private set; }

    /// <summary>
    /// 请求时的特定设置
    /// </summary>
    public RequestOption RequestOption { get; private set; }

    /// <summary>
    /// 超时时间(计算后)
    /// </summary>
    public int Timeout { get; internal set; } = 20000;

    /// <summary>
    /// 服务配置节
    /// </summary>
    public ServiceItem ServiceItem { get; private set; }

    /// <summary>
    /// 基础地址
    /// </summary>
    public string BaseUrl { get; private set; }

    /// <summary>
    /// 认证信息
    /// </summary>
    public Authorization? Authorization { get; private set; }

    /// <summary>
    /// Api配置节
    /// </summary>
    public ApiItem ApiItem { get; private set; }

    /// <summary>
    /// 是否需要缓存(计算后)
    /// </summary>
    public bool NeedCache { get; private set; }

    /// <summary>
    /// 最终的请求地址(计算后)
    /// </summary>
    public string FinalUrl { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public object? OriginParam { get; set; }

    /// <summary>
    /// 请求参数(转换为字典类型后)
    /// </summary>
    public Dictionary<string, string>? ParamDic { get; set; }

    /// <summary>
    /// 响应结果
    /// </summary>
    public string ResponseContent { get; private set; }

    /// <summary>
    /// 请求执行时间
    /// </summary>
    public int Runtime { get; private set; }

    /// <summary>
    /// 请求结果来源
    /// </summary>
    public string ResultFrom { get; internal set; } = "Request";

    /// <summary>
    /// 缓存Key
    /// </summary>
    public string CacheKey { get; private set; }

    /// <summary>
    /// 请求结果对象
    /// </summary>
    public ApiResult? ApiResult { get; internal set; }

    /// <summary>
    /// 请求体
    /// </summary>
    public HttpContent HttpContent { get; private set; }

    /// <summary>
    /// 缓存时间(分, 计算后)
    /// </summary>
    public int CacheMinuties { get; private set; }

    #endregion
}