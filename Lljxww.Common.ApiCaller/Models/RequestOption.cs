namespace Lljxww.Common.ApiCaller.Models
{
    public class RequestOption
    {
        /// <summary>
        /// 是否触发OnExecuted方法
        /// </summary>
        public bool IsTriggerOnExecuted { get; set; } = true;

        /// <summary>
        /// 是否从缓存读取结果(如果有)
        /// </summary>
        public bool IsFromCache { get; set; } = true;

        /// <summary>
        /// 自定义URL配置
        /// </summary>
        public Func<string, string> CustomFinalUrlHandler { get; set; }

        /// <summary>
        /// 自定义请求体
        /// </summary>
        public HttpContent CustomHttpContent { get; set; }

        /// <summary>
        /// 自定义认证信息
        /// </summary>
        public string CustomAuthorizeInfo { get; set; }

        /// <summary>
        /// 超时时长（ms），超过此时间的请求将取消
        /// </summary>
        public int Timeout { get; set; } = -1;
    }
}
