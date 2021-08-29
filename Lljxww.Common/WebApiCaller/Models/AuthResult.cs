using System;

namespace Lljxww.Common.WebApiCaller.Models
{
    /// <summary>
    /// 请求授权处理结果
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// 处理后的URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求的参数
        /// </summary>
        public object Param { get; set; }

        /// <summary>
        /// 是否成功执行授权工作
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 如果执行授权工作出错, 此处为出错的堆栈追踪信息
        /// </summary>
        public Exception InnerException { get; set; }
    }
}
