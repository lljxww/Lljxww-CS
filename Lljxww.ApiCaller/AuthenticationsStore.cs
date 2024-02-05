using Lljxww.ApiCaller.Models.Context;

namespace Lljxww.ApiCaller;

public static class AuthenticationsStore
{
    private static readonly Dictionary<string, Func<CallerContext, CallerContext>> AuthFuncs = [];

    /// <summary>
    /// 注册授权操作
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="func">操作Func(CallerContext, AuthResult)</param>
    public static void AddAuthFunc(string key, Func<CallerContext, CallerContext> func)
    {
        if (AuthFuncs.ContainsKey(key))
        {
            AuthFuncs[key] = func;
        }
        else
        {
            AuthFuncs.Add(key, func);
        }
    }

    /// <summary>
    /// 检查是否包含指定的认证配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool ContainsKey(string key)
    {
        key = key.Trim();
        return AuthFuncs.ContainsKey(key);
    }

    /// <summary>
    /// 在当前Caller上下文运行其指定的认证配置操作
    /// </summary>
    /// <param name="key"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static CallerContext Execute(string key, CallerContext context)
    {
        return AuthFuncs[key.Trim()].Invoke(context);
    }
}
