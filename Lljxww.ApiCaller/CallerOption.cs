namespace Lljxww.ApiCaller;

public class CallerOption
{
    public static string Version { get; } = "2.0";

    /// <summary>
    /// 请求中的User-Agent
    /// </summary>
    public static string UserAgent { get; } = $"Lljxww.ApiCaller/{Version}";

    #region 运行环境

    /// <summary>
    /// Caller的运行环境
    /// </summary>
    public static CallerEnvironment Environment { get; private set; }

    /// <summary>
    /// 配置ApiCaller的运行环境
    /// </summary>
    /// <param name="callerEnvironment"></param>
    public static void SetUpEnvironment(CallerEnvironment callerEnvironment)
    {
        Environment = callerEnvironment;
    }

    /// <summary>
    /// 配置ApiCaller的运行环境
    /// </summary>
    /// <param name="environment"></param>
    public static void SetupEnvironment(string environment)
    {
        if (string.IsNullOrWhiteSpace(environment))
        {
            Environment = CallerEnvironment.PRODUCTION;
            return;
        }

        environment = environment.ToLower();

        if (environment.StartsWith("dev"))
        {
            Environment = CallerEnvironment.DEVELOPMENT;
            return;
        }

        if (environment.StartsWith("stag"))
        {
            Environment = CallerEnvironment.STAGING;
            return;
        }

        if (environment.StartsWith("prod"))
        {
            Environment = CallerEnvironment.PRODUCTION;
            return;
        }

        Environment = CallerEnvironment.PRODUCTION;
    }

    #endregion
}

public enum CallerEnvironment
{
    /// <summary>
    /// 开发环境
    /// </summary>
    DEVELOPMENT,

    /// <summary>
    /// 测试环境
    /// </summary>
    STAGING,

    /// <summary>
    /// 生产环境
    /// </summary>
    PRODUCTION
}
