using System;
using System.Globalization;

namespace Lljxww.Common.Extensions;

public static class DateTimeExtension
{
    private static readonly DateTime BaseUtcTime = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// 将时间转换为Unix时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToUnixTimestamp(this DateTime dateTime)
    {
        string timestamp = (dateTime - BaseUtcTime).TotalSeconds.ToString(CultureInfo.InvariantCulture);
        return timestamp.Split('.')[0];
    }
}