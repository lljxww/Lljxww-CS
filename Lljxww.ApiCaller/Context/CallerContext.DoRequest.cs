using System.Diagnostics;

namespace Lljxww.ApiCaller.Models.Context;

public partial class CallerContext
{
    /// <summary>
    /// 使用当下的请求上下文发起请求
    /// </summary>
    /// <returns>附带请求结果的请求上下文</returns>
    public async Task<CallerContext> RequestAsync()
    {
        ResultFrom = "Request";

        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.CancelAfter(RequestContext.Timeout);

        Stopwatch sw = new();

        try
        {
            HttpClient client = HttpClientInstance.Get(this);

            sw.Start();

            HttpResponseMessage response = client.SendAsync(RequestMessage, cancellationTokenSource.Token).Result;
            ResponseContent = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(ResponseContent))
            {
                ApiResult = new ApiResult(ResponseContent, response, this);
            }
        }
        finally
        {
            sw.Stop();
            Runtime = Convert.ToInt32(sw.ElapsedMilliseconds);
        }

        return this;
    }
}
