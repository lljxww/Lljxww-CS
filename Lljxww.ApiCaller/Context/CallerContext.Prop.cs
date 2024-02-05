using Lljxww.ApiCaller.Context;

namespace Lljxww.ApiCaller.Models.Context;

public partial class CallerContext
{
    /// <summary>
    /// 请求上下文
    /// </summary>
    public RequestContext RequestContext { get; private set; } = new RequestContext();

    /// <summary>
    /// [代理]接口名
    /// </summary>
    public string ApiName => RequestContext.ApiName;

    /// <summary>
    /// 最终的请求地址(计算后)
    /// </summary>
    public string FinalUrl { get; set; }

    /// <summary>
    /// 请求参数(转换为字典类型后)
    /// </summary>
    public Dictionary<string, string>? ParamDic { get; set; }

    /// <summary>
    /// 请求信息对象
    /// </summary>
    public HttpRequestMessage RequestMessage { get; private set; }

    /// <summary>
    /// 请求体
    /// </summary>
    public HttpContent HttpContent { get; private set; }

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
    public string ResultFrom { get; set; } = "Request";

    /// <summary>
    /// 缓存Key
    /// </summary>
    public string CacheKey { get; private set; }

    /// <summary>
    /// 请求结果对象
    /// </summary>
    public ApiResult? ApiResult { get; set; }
}
