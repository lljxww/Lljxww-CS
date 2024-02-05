using Lljxww.ApiCaller.Context;

namespace Lljxww.ApiCaller.RequestContextLoader;

public interface IRequestContextLoader
{
    /// <summary>
    /// 加载RequestContext
    /// </summary>
    /// <param name="apiName"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    RequestContext LoadAsRequestContext(string apiName, object? param = null);
}
