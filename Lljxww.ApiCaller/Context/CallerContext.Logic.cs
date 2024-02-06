using Lljxww.ApiCaller.Config;
using Lljxww.ApiCaller.Context;
using Lljxww.ApiCaller.Exceptions;
using Lljxww.ApiCaller.Utils;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Lljxww.ApiCaller.Models.Context;

public partial class CallerContext
{
    /// <summary>
    /// 配置缓存
    /// </summary>
    /// <param name="context">请求上下文</param>
    /// <returns>配置好缓存相关信息的请求上下文</returns>
    private static CallerContext ConfigureCache(CallerContext context)
    {
        if (!context.RequestContext.NeedCache)
        {
            return context;
        }

        string keySource =
            $"{context.ApiName}+{(context.RequestContext.Param == null ? "" : JsonSerializer.Serialize(context.RequestContext.Param))}"
                .ToLower();

        if (!string.IsNullOrWhiteSpace(context.RequestContext.CustomCacheKeyPart))
        {
            keySource += context.RequestContext.CustomCacheKeyPart;
        }

        using SHA1 sha = SHA1.Create();
        byte[] result = sha.ComputeHash(Encoding.UTF8.GetBytes(keySource));
        context.CacheKey = $"caller:{BitConverter.ToString(result).Replace("-", "").ToLower()}";

        return context;
    }

    /// <summary>
    /// 配置请求参数
    /// </summary>
    /// <param name="context">请求上下文</param>
    /// <returns>配置好请求参数的请求上下文</returns>
    private static CallerContext ConfigureRequestParam(CallerContext context)
    {
        // 请求地址和请求头
        context.FinalUrl = context.RequestContext.Url;

        // 自定义最终的请求URL
        if (context.RequestContext!.CustomFinalUrlHandler != null)
        {
            context.FinalUrl = context.RequestContext!.CustomFinalUrlHandler.Invoke(context.FinalUrl);
        }

        context.RequestMessage = new HttpRequestMessage
        {
            Method = HttpMethodUtil.GetHttpMethod(context.RequestContext.HttpMethod),
            RequestUri = new Uri(context.FinalUrl),
            Content = context.HttpContent
        };

        switch (context.RequestContext.ParamType)
        {
            case ParamPosition.QueryString:
                {
                    if (context.ParamDic?.Count > 0)
                    {
                        if (!context.FinalUrl.Contains('?'))
                        {
                            context.FinalUrl += "?";
                        }

                        foreach (KeyValuePair<string, string> keyValuePair in context.ParamDic)
                        {
                            if (context.RequestContext.EncodeUrl)
                            {
                                context.FinalUrl += $"&{keyValuePair.Key}={HttpUtility.UrlEncode(keyValuePair.Value)}";
                            }
                            else
                            {
                                context.FinalUrl += $"&{keyValuePair.Key}={keyValuePair.Value}";
                            }
                        }

                        context.FinalUrl = context.FinalUrl.Replace("?&", "?");
                    }

                    break;
                }
            case ParamPosition.Path:
                {
                    if (context.ParamDic != null)
                    {
                        foreach (KeyValuePair<string, string> keyValuePair in context.ParamDic)
                        {
                            context.FinalUrl = context.FinalUrl.Replace($"{{{keyValuePair.Key}}}",
                                context.RequestContext.EncodeUrl ? HttpUtility.UrlEncode(keyValuePair.Value) : keyValuePair.Value);
                        }
                    }

                    break;
                }
            case ParamPosition.Body:
                {
                    if (context.RequestContext.Param == null && context.RequestContext == null)
                    {
                        break;
                    }

                    if (context.RequestContext.CustomHttpContentHandler != null)
                    {
                        context.HttpContent = context.RequestContext.CustomHttpContentHandler.Invoke(context.HttpContent);
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(context.RequestContext.ContentType))
                    {
                        context.HttpContent = new StringContent(JsonSerializer.Serialize(context.RequestContext.Param));
                        context.HttpContent.Headers.ContentType = new MediaTypeHeaderValue(context.RequestContext.ContentType);
                    }
                    else
                    {
                        if (context.ParamDic != null)
                        {
                            context.HttpContent = new FormUrlEncodedContent(context.ParamDic!);
                        }
                    }

                    break;
                }
            case ParamPosition.Header:
                {
                    break;
                }
        }

        return context;
    }

    /// <summary>
    /// 配置接口授权
    /// </summary>
    /// <param name="config">接口调用配置</param>
    /// <param name="context">请求上下文</param>
    /// <returns>配置好接口授权的请求上下文</returns>
    /// <exception cref="CallerException"></exception>
    private static CallerContext ConfigureAuth(CallerContext context)
    {
        if (string.IsNullOrWhiteSpace(context.RequestContext.AuthorizationName))
        {
            return context;
        }

        if (!AuthenticationsStore.ContainsKey(context.RequestContext.AuthorizationName))
        {
            throw new CallerException($"找不到认证配置：{context.RequestContext.AuthorizationName}");
        }

        return AuthenticationsStore.Execute(context.RequestContext.AuthorizationName, context);
    }
}