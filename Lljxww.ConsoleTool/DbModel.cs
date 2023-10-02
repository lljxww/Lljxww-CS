namespace Lljxww.ConsoleTool;

internal class DbModel
{
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; } = "0.2";

    /// <summary>
    /// 静默模式，不输出过多信息
    /// </summary>
    public bool SilentMode { get; set; } = true;

    /// <summary>
    /// 系统保存的最多的CallerConfigVersion数量
    /// </summary>
    public int MaxCallerConfigCount { get; set; } = 10;

    /// <summary>
    /// 配置文件信息
    /// </summary>
    public IList<CallerConfigInfo> CallerConfigInfos { get; set; } = new List<CallerConfigInfo>();
}

internal class CallerConfigInfo
{
    /// <summary>
    /// 标签
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// 文件所在目录
    /// </summary>
    public string Directory { get; set; }

    /// <summary>
    /// 文件路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; } = DateTime.Now;

    /// <summary>
    /// 是否激活状态
    /// </summary>
    public bool Active { get; set; }
}
