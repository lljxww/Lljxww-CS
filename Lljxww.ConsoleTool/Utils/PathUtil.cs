namespace Lljxww.ConsoleTool;

internal class PathUtil
{
    /// <summary>
    /// 程序数据文件的路径
    /// </summary>
    internal static string FileDirectory => Path.Combine(Environment.CurrentDirectory, "config");

    /// <summary>
    /// Caller配置文件目录
    /// </summary>
    internal static string CallerConfigFileDirectory => Path.Combine(FileDirectory, "apicallerconfig");

    /// <summary>
    /// 配置文件的目录路径
    /// </summary>
    internal static string DbModelFilePath => Path.Combine(FileDirectory, "setting.json");

    internal static string CALLER_CONFIG_FILE_NAME = "caller.json";
}
