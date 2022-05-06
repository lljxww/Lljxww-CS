using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Lljxww.Common.Utilities.Image;
#pragma warning disable CA1416 // 验证平台兼容性
public class VerificationCode
{
    /// <summary>
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string CreateValidateCodeWithLetter(int length)
    {
        //字母去掉I、O避免与数字混淆
        char[] pattern =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
            'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y'
        };

        string result = "";
        int n = pattern.Length;
        Random random = new(~unchecked((int)DateTime.Now.Ticks));
        for (int i = 0; i < length; i++)
        {
            int rnd = random.Next(0, n);
            result += pattern[rnd];
        }

        return result;
    }

    /// <summary>
    ///     创建验证码图形的字节流
    /// </summary>
    /// <param name="validateCode">验证码</param>
    /// <returns>图形字节流</returns>
    public static byte[] CreateValidateGraphic(string validateCode)
    {
        Bitmap image = new(100, 44);
        Graphics g = Graphics.FromImage(image);
        try
        {
            //生成随机生成器
            Random random = new();
            //清空图片背景色
            g.Clear(Color.White);
            //画图片的干扰线
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            Font font = new("Arial", 16, FontStyle.Bold | FontStyle.Italic);
            LinearGradientBrush brush = new(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed,
                1.2f, true);
            g.DrawString(validateCode, font, brush, 18, 10);

            //画图片的前景干扰点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            //保存图片数据
            MemoryStream stream = new();
            image.Save(stream, ImageFormat.Jpeg);

            //输出图片流
            return stream.ToArray();
        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }
    }
}
#pragma warning restore CA1416 // 验证平台兼容性