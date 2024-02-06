using Lljxww.ApiCaller.Context;
using Lljxww.ApiCaller.RequestContextLoader;
using Microsoft.Extensions.DependencyInjection;

namespace Lljxww.ApiCaller.Utils;

public class RequestContextUtil
{
    private readonly IRequestContextLoader _configRequestContextLoader;
    private readonly IRequestContextLoader _swaggerJsonFileRequestContextLoader;
    private readonly IRequestContextLoader _swaggerWebRequestContextLoader;
    private readonly IRequestContextLoader _standaloneRequestContextLoader;

    public RequestContextUtil([FromKeyedServices("config")] IRequestContextLoader configRequestContextLoader,
        [FromKeyedServices("swagger-json-file")] IRequestContextLoader swaggerJsonFileRequestContextLoader,
        [FromKeyedServices("swagger-web")] IRequestContextLoader swaggerWebRequestContextLoader,
        [FromKeyedServices("standalone")] IRequestContextLoader standaloneRequestContextLoader)
    {
        _configRequestContextLoader = configRequestContextLoader;
        _swaggerJsonFileRequestContextLoader = swaggerJsonFileRequestContextLoader;
        _swaggerWebRequestContextLoader = swaggerWebRequestContextLoader;
        _standaloneRequestContextLoader = standaloneRequestContextLoader;
    }

    public RequestContext BuildRequestContext(string name, object? param = null, RequestOption? requestOption = null,
        RequestContextSource source = RequestContextSource.CONFIG_FILE)
    {
        switch (source)
        {
            case RequestContextSource.CONFIG_FILE:
                {
                    return _configRequestContextLoader.LoadAsRequestContext(name, param, requestOption);
                }
            case RequestContextSource.SWAGGER_JSON_FILE:
                {
                    return _swaggerJsonFileRequestContextLoader.LoadAsRequestContext(name, param, requestOption);
                }
            case RequestContextSource.SWAGGER_WEB:
                {
                    return _swaggerWebRequestContextLoader.LoadAsRequestContext(name, param, requestOption);
                }
            case RequestContextSource.STANDALONE:
                {
                    return _standaloneRequestContextLoader.LoadAsRequestContext(name, param, requestOption);
                }
            default:
                {
                    throw new NotImplementedException();
                }
        }
    }
}
