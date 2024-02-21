namespace Lljxww.ApiCaller.Exceptions;

/// <summary>
/// 找不到指定的方法
/// </summary>
/// <param name="message"></param>
public class ItemNotFoundException(string message) : CallerException(message)
{
}
