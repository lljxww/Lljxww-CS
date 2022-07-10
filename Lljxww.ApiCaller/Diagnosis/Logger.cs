using System.Text;
using System.Text.Json;

namespace Lljxww.ApiCaller.Diagnosis;

public static class Logger
{
    public static async Task LogAsync(CallerContext context)
    {
        StringBuilder sb = new();

        sb.Append($"Method: {context.ApiName}{Environment.NewLine}");
        sb.Append($"Parameters: {JsonSerializer.Serialize(context.OriginParam)}{Environment.NewLine}");
        sb.Append($"Result: {context.ApiResult?.RawStr}");
        sb.Append($"RunTime: {context.Runtime}{Environment.NewLine}");
        sb.Append($"Authorization: {context.Authorization?.Name}{Environment.NewLine}");
        sb.Append($"Authorization Info: {context.Authorization?.AuthorizationInfo}{Environment.NewLine}");
        sb.Append($"Result From: {context.ResultFrom}{Environment.NewLine}");
        sb.Append($"Url: {context.FinalUrl}");
        //TODO Event Results

        sb.Append(Environment.NewLine);

        await LogAsync(sb.ToString());
    }
    
    private static async Task LogAsync(string content)
    {
        try
        {
            var directoryPath = InitDirectory();
            var fileName = $"caller.{DateTime.Now:yyyyMMddHHmm}.log";
            var filePath = Path.Combine(directoryPath, fileName);

            using var sw = new StreamWriter(filePath, true, Encoding.UTF8);
            await sw.WriteLineAsync(content);
        }
        catch(Exception)
        {
            // ignored
        }
    }

    private static string InitDirectory()
    {
        var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Diagnosis");
        if(!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        return directoryPath;
    }
}