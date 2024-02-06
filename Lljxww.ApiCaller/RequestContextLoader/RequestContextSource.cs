namespace Lljxww.ApiCaller.RequestContextLoader;

/// <summary>
/// 构造RequestContext的来源
/// </summary>
public enum RequestContextSource
{
    CONFIG_FILE,

    STANDALONE,

    SWAGGER_JSON_FILE,

    SWAGGER_WEB,
}
