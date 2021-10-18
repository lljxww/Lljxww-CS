using System;
using System.Text.RegularExpressions;

namespace Lljxww.Common.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 手机号码验证
        /// </summary>
        private static readonly Regex _mobileNumberRegex = new("/^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\\d{8}$/");

        /// <summary>
        /// 检查字符串是否是手机号码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsMobileNumber(this string source)
        {
            return _mobileNumberRegex.IsMatch(source);
        }

        public static bool IsIDNumber(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }

            int length = source.Length;

            if (length == 18)
            {
                return CheckIDCard18(source);
            }

            if (length == 15)
            {
                return CheckIDCard15(source);
            }

            return false;
        }

        private static bool CheckIDCard15(string Id)
        {
            if (long.TryParse(Id, out long n) == false || n < Math.Pow(10, 14))

            {
                return false;//数字验证

            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (!address.Contains(Id.Remove(2), StringComparison.CurrentCulture))
            {
                return false;//省份验证
            }

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            if (DateTime.TryParse(birth, out _) == false)
            {
                return false;//生日验证
            }

            return true;//符合15位身份证标准
        }

        private static bool CheckIDCard18(string Id)
        {
            if (long.TryParse(Id.Remove(17), out long n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out _) == false)
            {
                return false;//数字验证
            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (!address.Contains(Id.Remove(2), StringComparison.CurrentCulture))
            {
                return false;//省份验证
            }

            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            if (DateTime.TryParse(birth, out _) == false)
            {
                return false;//生日验证
            }

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;

            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }

            Math.DivRem(sum, 11, out int y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                //校验码验证
                return false;
            }

            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 截取字符串左边指定长度的子字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetLeft(this string source, int count)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return source;
            }

            if (count <= 0)
            {
                return "";
            }

            if (source.Length <= count)
            {
                return source;
            }

            return source.Substring(0, count);
        }

        /// <summary>
        /// 截取字符串右边指定长度的子字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetRight(this string source, int count)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return source;
            }

            if (count <= 0)
            {
                return "";
            }

            if (source.Length <= count)
            {
                return source;
            }

            return source.Substring(source.Length - count, count);
        }
    }
}
