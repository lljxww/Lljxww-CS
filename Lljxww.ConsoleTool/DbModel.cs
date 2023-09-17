namespace Lljxww.ConsoleTool;

public class DbModel
{
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; } = "0.1";

    /// <summary>
    /// 外挂配置文件的路径
    /// </summary>
    public string CallerConfigPath { get; set; } = string.Empty;

    /// <summary>
    /// 静默模式，不输出过多信息
    /// </summary>
    public bool SilentMode { get; set; } = true;
}
