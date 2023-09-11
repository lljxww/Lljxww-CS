namespace Lljxww.ConsoleTool;

public class ActionResult<T>
{
    public bool Success { get; set; }

    public T Content { get; set; }

    public string Message { get; set; }
}
