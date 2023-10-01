namespace Lljxww.ConsoleTool;

public class PathUtil
{
    /// <summary>
    /// 程序数据文件的路径
    /// </summary>
    internal static string FileDirectory => Path.Combine(Environment.CurrentDirectory, "config");

    internal static string CallerConfigFileDirectory => Path.Combine(FileDirectory, "apicallerconfig");

    /// <summary>
    /// 配置文件的目录路径
    /// </summary>
    internal static string FilePath => Path.Combine(FileDirectory, "setting.json");
}
