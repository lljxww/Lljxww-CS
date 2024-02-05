using System.Text.Json.Serialization;

namespace Lljxww.ApiCaller.Config;

public class ServiceItem : ICallerConfigNode
{
    [JsonIgnore]
    public Guid ID { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// 接口名
    /// </summary>
    public string ApiName { get; set; }

    /// <summary>
    /// 接口授权类型, 目前支持ecp, sign; multisign; accesstoken四种
    /// </summary>
    public string AuthorizationType { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// 超时时间
    /// </summary>
    public int Timeout { get; set; }

    /// <summary>
    /// 接口配置节
    /// </summary>
    public IList<ApiItem> ApiItems { get; set; }

    public Guid GetID()
    {
        return ID;
    }

    public string Remark()
    {
        return ApiName;
    }
}