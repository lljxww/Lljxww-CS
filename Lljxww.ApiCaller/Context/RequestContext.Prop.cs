using Lljxww.ApiCaller.Models.Context;
using System.Text.Json.Serialization;

namespace Lljxww.ApiCaller.Context;

public partial class RequestContext
{
    internal RequestContext() { }

    /// <summary>
    /// 请求的唯一ID
    /// </summary>
    [JsonIgnore]
    public string ID { get; private set; } = DateTime.Now.ToString("yyyyMMddHHmmssffff");

    /// <summary>
    /// 运行环境
    /// </summary>
    public string Environment { get; set; } = "prod";

    /// <summary>
    /// 接口名
    /// </summary>
    public string ApiName { get; set; }

    /// <summary>
    /// 方法描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 接口授权类型
    /// </summary>
    public string AuthorizationName { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public string HttpMethod { get; set; }

    /// <summary>
    /// 请求参数, 建议使用字典或对象/匿名对象
    /// </summary>
    public object? Param { get; set; }

    /// <summary>
    /// 方法参数类型
    /// </summary>
    public ParamPosition ParamType { get; set; }

    /// <summary>
    /// 请求头中的Content-Type, 默认值: application/json
    /// </summary>
    public string ContentType { get; set; } = "application/json";

    /// <summary>
    /// 超时时间(毫秒)
    /// </summary>
    public int Timeout { get; set; }

    /// <summary>
    /// 是否需要缓存
    /// </summary>
    public bool NeedCache { get; set; } = false;

    /// <summary>
    /// 缓存时长(分钟)
    /// </summary>
    public int CacheTime { get; set; } = 10;

    /// <summary>
    /// 是否对URL进行编码
    /// </summary>
    public bool EncodeUrl { get; set; } = true;

    /// <summary>
    /// 是否触发OnExecuted方法
    /// </summary>
    public bool IsTriggerOnExecuted { get; set; } = true;

    /// <summary>
    /// 是否从缓存读取结果(如果有)
    /// </summary>
    public bool IsFromCache { get; set; } = true;

    /// <summary>
    /// 自定义URL配置, 通过此方法处理最终请求的URL
    /// </summary>
    public Func<string, string>? CustomFinalUrlHandler { get; set; }

    /// <summary>
    /// 自定义请求体
    /// </summary>
    public Func<HttpContent, HttpContent>? CustomHttpContentHandler { get; set; }

    /// <summary>
    /// 不记录日志
    /// </summary>
    public bool DontLog { get; set; } = false;

    /// <summary>
    /// 当此predicate值为true时, 不将结果写入Cache
    /// </summary>
    public Predicate<CallerContext> WhenDontSaveRequestCache { get; set; } = _ => false;

    /// <summary>
    /// 自定义认证信息
    /// </summary>
    public string? CustomAuthorizeInfo { get; set; }

    /// <summary>
    /// 自定义参与计算缓存key的字符串
    /// </summary>
    /// <remarks>
    /// 某些请求的业务参数依赖授权信息，
    /// 如放到header中的某些字符串。
    /// 默认的缓存key计算逻辑并不会读取到header中的信息，
    /// 所以可通过此字段添加特定的业务参数来对不同的请求加以区分
    /// </remarks>
    public string? CustomCacheKeyPart { get; set; }

    /// <summary>
    /// 自定义对象, 可用于将请求时的一些细节传递到各类事件处理程序中使用
    /// </summary>
    public object? CustomObject { get; set; }
}

/// <summary>
/// 参数的位置
/// </summary>
public enum ParamPosition
{
    /// <summary>
    /// 无参数
    /// </summary>
    None,

    /// <summary>
    /// 查询字符串
    /// </summary>
    QueryString,

    /// <summary>
    /// 请求体
    /// </summary>
    Body,

    /// <summary>
    /// 路径
    /// </summary>
    Path,

    /// <summary>
    /// 请求头
    /// </summary>
    Header
}