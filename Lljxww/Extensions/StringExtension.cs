using System;
using System.Text.RegularExpressions;

namespace Lljxww.Extensions;

public static class StringExtension
{
    /// <summary>
    /// 手机号码验证
    /// </summary>
    private static readonly Regex MobileNumberRegex =
        new("/^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\\d{8}$/");

    /// <summary>
    /// 检查字符串是否是手机号码
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsMobileNumber(this string source)
    {
        return MobileNumberRegex.IsMatch(source);
    }

    public static bool IsIdNumber(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return false;
        }

        int length = source.Length;

        return length switch
        {
            18 => CheckIdCard18(source),
            15 => CheckIdCard15(source),
            _ => false
        };
    }

    private static bool CheckIdCard15(string id)
    {
        if (long.TryParse(id, out long n) == false || n < Math.Pow(10, 14))

        {
            return false; //数字验证
        }

        const string address =
            "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

        if (!address.Contains(id.Remove(2), StringComparison.CurrentCulture))
        {
            return false; //省份验证
        }

        string birth = id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
        return DateTime.TryParse(birth, out _);
    }

    private static bool CheckIdCard18(string id)
    {
        if (long.TryParse(id.Remove(17), out long n) == false || n < Math.Pow(10, 16) ||
            long.TryParse(id.Replace('x', '0').Replace('X', '0'), out _) == false)
        {
            return false; //数字验证
        }

        const string address =
            "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

        if (!address.Contains(id.Remove(2), StringComparison.CurrentCulture))
        {
            return false; //省份验证
        }

        string birth = id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
        if (DateTime.TryParse(birth, out _) == false)
        {
            return false; //生日验证
        }

        string[] arrVerifyCode = "1,0,x,9,8,7,6,5,4,3,2".Split(',');
        string[] wi = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(',');
        char[] ai = id.Remove(17).ToCharArray();
        int sum = 0;

        for (int i = 0; i < 17; i++)
        {
            sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
        }

        Math.DivRem(sum, 11, out int y);
        return arrVerifyCode[y] == id.Substring(17, 1).ToLower();
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

        return source.Length <= count ? source : source[..count];
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

        return source.Length <= count ? source : source.Substring(source.Length - count, count);
    }
}