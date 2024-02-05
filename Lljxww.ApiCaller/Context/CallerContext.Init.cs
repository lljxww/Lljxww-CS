using Lljxww.ApiCaller.Context;
using Lljxww.ApiCaller.Extensions;

namespace Lljxww.ApiCaller.Models.Context;

public partial class CallerContext
{
    private CallerContext() { }

    public static CallerContext Build(RequestContext context)
    {
        CallerContext callerContext = new()
        {
            RequestContext = context,
            ParamDic = context.Param?.AsDictionary(),
        };

        callerContext = ConfigureCache(callerContext);
        callerContext = ConfigureRequestParam(callerContext);
        callerContext = ConfigureAuth(callerContext);

        return callerContext;
    }
}