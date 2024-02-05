using Lljxww.ApiCaller.Config;

namespace Lljxww.ConsoleTool.Models;

public class NodeEditModel
{
    public ICallerConfigNode Node { get; set; }

    public int Index { get; set; }

    public int ParentIndex { get; set; } = -1;

    public Type Type { get; set; }
}