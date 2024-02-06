using Lljxww.ApiCaller.Context;

namespace Lljxww.ApiCaller.Utils;

internal class HttpMethodUtil
{
    public static HttpMethod GetHttpMethod(EHttpMethod method)
    {
        switch (method)
        {
            case EHttpMethod.GET:
                {
                    return HttpMethod.Get;
                }
            case EHttpMethod.POST:
                {
                    return HttpMethod.Post;
                }
            case EHttpMethod.PUT:
                {
                    return HttpMethod.Put;
                }
            case EHttpMethod.DELETE:
                {
                    return HttpMethod.Delete;
                }
            default:
                {
                    throw new NotImplementedException();
                }
        }
    }
}
