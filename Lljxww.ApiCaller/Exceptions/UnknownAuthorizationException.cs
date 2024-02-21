namespace Lljxww.ApiCaller.Exceptions;

/// <summary>
/// 找不到指定的授权配置
/// </summary>
/// <param name="message"></param>
public class UnknownAuthorizationException(string message) : CallerException(message)
{
}
