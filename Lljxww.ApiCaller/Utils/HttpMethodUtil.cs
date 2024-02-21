namespace Lljxww.ApiCaller.Utils;

internal class HttpMethodUtil
{
    public static HttpMethod GetHttpMethod(string method)
    {
        switch (method.Trim().ToUpper())
        {
            case "GET":
                {
                    return HttpMethod.Get;
                }
            case "POST":
                {
                    return HttpMethod.Post;
                }
            case "PUT":
                {
                    return HttpMethod.Put;
                }
            case "DELETE":
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
