using Lljxww.ApiCaller.Models;
using System.Text;
using System.Text.Json;

namespace Lljxww.ApiCaller.Diagnosis;

public static class Logger
{
    public static void Log(CallerContext context)
    {
        StringBuilder sb = new();

        _ = sb.Append($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        _ = sb.Append($"Method: {context.ApiName}{Environment.NewLine}");
        _ = sb.Append($"Parameters: {JsonSerializer.Serialize(context.OriginParam)}{Environment.NewLine}");
        _ = sb.Append($"Result: {context.ApiResult?.RawStr}");
        _ = sb.Append($"RunTime: {context.Runtime}{Environment.NewLine}");
        _ = sb.Append($"Authorization: {context.Authorization?.Name}{Environment.NewLine}");
        _ = sb.Append($"Authorization Info: {context.Authorization?.AuthorizationInfo}{Environment.NewLine}");
        _ = sb.Append($"Result From: {context.ResultFrom}{Environment.NewLine}");
        _ = sb.Append($"Url: {context.FinalUrl}");
        //TODO Event Results

        _ = sb.Append(Environment.NewLine);

        _ = Task.Run(() => LogAsync(sb.ToString()));
    }

    private static async Task LogAsync(string content)
    {
        try
        {
            string directoryPath = InitDirectory();
            string fileName = $"caller.{DateTime.Now:yyyyMMddHHmm}.log";
            string filePath = Path.Combine(directoryPath, fileName);

            using StreamWriter sw = new(filePath, true, Encoding.UTF8);
            await sw.WriteLineAsync(content);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private static string InitDirectory()
    {
        string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Diagnosis");
        if (!Directory.Exists(directoryPath))
        {
            _ = Directory.CreateDirectory(directoryPath);
        }

        return directoryPath;
    }
}