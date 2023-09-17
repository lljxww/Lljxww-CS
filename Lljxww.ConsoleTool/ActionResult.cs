namespace Lljxww.ConsoleTool;

public class ActionResult
{
    public bool Success { get; set; }

    public string Message { get; set; }
}

public class ActionResult<T> : ActionResult
{
    public T Content { get; set; }
}