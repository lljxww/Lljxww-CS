using Lljxww.ApiCaller.Context;

namespace Lljxww.ApiCaller.RequestContextLoader;

public interface IRequestContextLoader
{
    /// <summary>
    /// 加载RequestContext
    /// </summary>
    /// <param name="name"></param>
    /// <param name="param"></param>
    /// <param name="requestOption"></param>
    /// <returns></returns>
    RequestContext LoadAsRequestContext(string name, object? param = null, RequestOption? requestOption = null);
}
