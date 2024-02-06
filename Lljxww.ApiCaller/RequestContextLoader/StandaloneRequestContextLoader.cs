using Lljxww.ApiCaller.Context;

namespace Lljxww.ApiCaller.RequestContextLoader;

internal class StandaloneRequestContextLoader : IRequestContextLoader
{
    public RequestContext LoadAsRequestContext(string name, object? param = null, RequestOption? requestOption = null)
    {
        throw new NotImplementedException();
    }
}
