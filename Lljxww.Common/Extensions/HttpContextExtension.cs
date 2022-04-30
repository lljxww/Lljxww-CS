using System;
using Microsoft.AspNetCore.Http;

namespace Lljxww.Common.Extensions;

public static class HttpContextExtension
{
    public static string? GetFromQueryString(this HttpContext context, string key)
    {
        if (context == null)
        {
            return null;
        }

        return context.Request.Query.Keys.Contains(key) 
            ? context.Request.Query[key].ToString() 
            : null;
    }

    public static string? GetFromHeader(this HttpContext context, string key)
    {
        if (context == null)
        {
            return null;
        }

        return context.Request.Headers.Keys.Contains(key)
            ? context.Request.Headers[key].ToString()
            : null;
    }
}