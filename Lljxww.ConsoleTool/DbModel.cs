namespace Lljxww.ConsoleTool;

public class DbModel
{
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; } = "0.1";

    /// <summary>
    /// 静默模式，不输出过多信息
    /// </summary>
    public bool SilentMode { get; set; } = true;

    /// <summary>
    /// Caller配置文件版本
    /// </summary>
    public int CallerConfigVersion { get; set; }

    /// <summary>
    /// 系统保存的最多的CallerConfigVersion数量
    /// </summary>
    public int MaxCallerConfigVersion { get; set; } = 10;
}
