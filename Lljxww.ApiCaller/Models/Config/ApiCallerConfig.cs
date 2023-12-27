using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace Lljxww.ApiCaller.Models.Config;

/// <summary>
/// 接口配置文件
/// </summary>
public class ApiCallerConfig : IOptions<ApiCallerConfig>
{
    /// <summary>
    /// 授权方式
    /// </summary>
    public IList<Authorization> Authorizations { get; set; }

    /// <summary>
    /// 接口配置节
    /// </summary>
    public IList<ServiceItem> ServiceItems { get; set; }

    public DiagnosisConfig? Diagnosis { get; set; }

    public string UserAgent { get; set; } = "Lljxww.WebApiCaller";

    [JsonIgnore]
    public ApiCallerConfig Value => this;
}