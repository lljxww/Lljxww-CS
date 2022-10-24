using Lljxww.ApiCaller.Diagnosis;
using Lljxww.ApiCaller.Models;
using Lljxww.ApiCaller.Models.Config;
using Microsoft.Extensions.Options;
using System.Net;

namespace Lljxww.ApiCaller;

public partial class Caller
{
    public async Task<ApiResult> InvokeAsync(string apiNameAndMethodName, object? requestParam = null,
        RequestOption? requestOption = null)
    {
        requestOption ??= new RequestOption();

        // 创建请求对象
        CallerContext context =
            CallerContext.Build(apiNameAndMethodName, _apiCallerConfig, requestParam, requestOption);

        // 尝试从缓存读取结果
        if (context.NeedCache && requestOption.IsFromCache)
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
                // 设置本次请求的超时时间（如果有）
                if (requestOption.Timeout > 0)
                {
                    context.Timeout = requestOption.Timeout;
                }

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
            if (context.NeedCache)
            {
                CallerEvents.SetCache(context);
            }
        }

        // 记录日志事件
        if (!requestOption.DontLog)
        {
            CallerEvents.Log(context);
        }

        // 执行后事件
        if (requestOption.IsTriggerOnExecuted)
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
}