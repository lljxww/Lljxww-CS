namespace Lljxww.Common.WebApiCaller.Models.Config
{
    /// <summary>
    /// 接口配置节
    /// </summary>
    public class ApiItem
    {
        /// <summary>
        /// 方法名
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 方法调用的接口url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 方法的Http谓词
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 方法参数类型
        /// </summary>
        public string ParamType { get; set; }

        /// <summary>
        /// 方法描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 日志类型, 支持file和mysql
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// 是否需要缓存
        /// </summary>
        public bool NeedCache { get; set; } = false;

        /// <summary>
        /// 缓存时长(分钟)
        /// </summary>
        public int CacheTime { get; set; } = 10;

        /// <summary>
        /// 作为参数时，参数的类型（默认为application/x-www-form-urlencoded, 支持application/json)
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 表示调用该方法时，需要清空哪些方法产生的缓存（如果有）。多个方法使用半角逗号分隔
        /// </summary>
        public string MethodCacheToRemove { get; set; }

        /// <summary>
        /// 接口授权类型, 目前支持ecp, sign; multisign; accesstoken四种
        /// </summary>
        public string AuthorizationType { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; }
    }
}
