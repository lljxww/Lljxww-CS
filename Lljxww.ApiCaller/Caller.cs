using Lljxww.ApiCaller.Config;
using Lljxww.ApiCaller.Context;
using Lljxww.ApiCaller.Diagnosis;
using Lljxww.ApiCaller.Exceptions;
using Lljxww.ApiCaller.Models.Context;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace Lljxww.ApiCaller;

public partial class Caller
{
    public async Task<ApiResult> InvokeAsync(string apiName, object? param = null, RequestOption? requestOption = null)
    {
        RequestContext requestContext = new()
        {
            ApiName = apiName.Trim(),
            Param = param
        };

        // 创建请求对象
        CallerContext context = CallerContext.Build(requestContext);

        // 尝试从缓存读取结果
        if (context.RequestContext.NeedCache && context.RequestContext.IsFromCache)
        {
            ApiResult? tempApiResult = CallerEvents.GetCache(context);

            if (tempApiResult != null)
            {
                context.ResultFrom = "Cache";
                context.ApiResult = tempApiResult!;
            }
        }

        // 从新Http请求获取结果
        if (context.ApiResult == null)
        {
            try
            {
                // 执行请求
                context = await context.RequestAsync();
            }
            catch (Exception ex)
            {
                Exception innerException = ex;

                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;
                }

                string? fullName = innerException.GetType().FullName;
                if (fullName != null && fullName.Contains(nameof(TaskCanceledException)))
                {
                    // 取消请求后的操作
                    CallerEvents.RequestTimeout(context);

                    return new ApiResult(new
                    {
                        success = false,
                        message = "目标服务超时"
                    }, null, context);
                }

                CallerEvents.Exception(context, innerException);
                throw innerException;
            }

            // 处理缓存
            if (context.RequestContext.NeedCache && !context.RequestContext.WhenDontSaveRequestCache.Invoke(context))
            {
                CallerEvents.SetCache(context);
            }
        }

        // 记录日志事件
        if (!context.RequestContext.DontLog)
        {
            CallerEvents.Log(context);
        }

        // 执行后事件
        if (context.RequestContext.IsTriggerOnExecuted)
        {
            CallerEvents.Executed(context);
        }

        ApiResult apiResult = context.ApiResult!;
        apiResult.Context = context;

        if (_apiCallerConfig.Diagnosis?.LogDetail != default
            && _apiCallerConfig.Diagnosis.LogDetail.Contains(context.ApiName))
        {
            Logger.Log(context);
        }

        return apiResult;
    }
}

public partial class Caller
{
    private readonly ApiCallerConfig _apiCallerConfig;

    public Caller(IOptions<ApiCallerConfig> apiCallerConfigOption)
    {
        _apiCallerConfig = apiCallerConfigOption.Value;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }

    public Caller(string configFilePath)
    {
        if (!File.Exists(configFilePath))
        {
            throw new CallerException($"config not found: {configFilePath}");
        }

        try
        {
            string jsonText = File.ReadAllText(configFilePath);
            _apiCallerConfig = JsonSerializer.Deserialize<ApiCallerConfig>(jsonText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new CallerException($"bad config: {configFilePath}");
        }
        catch (Exception ex)
        {
            throw new CallerException($"bad config: {ex.Message}");
        }
    }

    /// <summary>
    /// 检查指定的目标是否存在
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool HasTarget(string target)
    {
        string[] items = target.Split('.');
        return items.Length == 2
            && _apiCallerConfig.ServiceItems.Any(s => s.ApiName != items[0])
            && _apiCallerConfig.ServiceItems.Single(s => s.ApiName == items[0])
                .ApiItems.Any(a => a.Method == items[1]);
    }
}
