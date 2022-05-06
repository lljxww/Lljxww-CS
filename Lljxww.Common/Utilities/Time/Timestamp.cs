using System;
using System.Globalization;

namespace Lljxww.Common.Utilities.Time;

/// <summary>
///     时间戳工具
/// </summary>
public static class Timestamp
{
    private static readonly DateTime BaseUtcTime = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    ///     查询当前的UNIX时间戳
    /// </summary>
    /// <returns>当前的UNIX时间戳</returns>
    public static string Get()
    {
        string timestamp = (DateTime.UtcNow - BaseUtcTime).TotalSeconds.ToString(CultureInfo.InvariantCulture);
        return timestamp.Split('.')[0];
    }

    /// <summary>
    ///     检查指定的时间戳是否已超过指定期限(分钟)
    /// </summary>
    /// <param name="timestamp">目标时间戳</param>
    /// <param name="expire">有效期(分)</param>
    /// <returns>是否已过期</returns>
    public static bool IsExpired(string timestamp, int expire)
    {
        string currentTimestamp = Get();

        try
        {
            return Math.Abs(Convert.ToInt32(timestamp) - Convert.ToInt32(currentTimestamp)) >= expire * 60;
        }
        catch
        {
            return false;
        }
    }
}