namespace Lljxww.ConsoleTool.Utils;

internal class PathUtil
{
    /// <summary>
    /// 程序数据文件的路径
    /// </summary>
    internal static string AppConfigFileDirectory => Path.Combine(Environment.CurrentDirectory, "config");

    /// <summary>
    /// Caller配置文件目录
    /// </summary>
    internal static string CallerConfigFileDirectory => Path.Combine(AppConfigFileDirectory, "apicallerconfig");

    /// <summary>
    /// 配置文件的目录路径
    /// </summary>
    internal static string DbModelFilePath => Path.Combine(AppConfigFileDirectory, "setting.json");

    /// <summary>
    /// caller的配置文件名
    /// </summary>
    internal static string CALLER_CONFIG_FILE_NAME = "caller.json";
}
