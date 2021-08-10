using System;

namespace Lljxww.Common.Extensions
{
    public static class DateTimeExtension
    {
        private static readonly DateTime BASE_UTC_TIME = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        //TODO 转移到Common
        /// <summary>
        /// 将时间转换为Unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToUnixTimestamp(this DateTime dateTime)
        {
            string timestamp = (dateTime - BASE_UTC_TIME).TotalSeconds.ToString();
            return timestamp.Split('.')[0];
        }
    }
}
