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
        /// 不记录日志
        /// </summary>
        public bool DontLog { get; set; } = false;

        /// <summary>
        /// 自定义认证信息
        /// </summary>
        public string CustomAuthorizeInfo { get; set; }

        /// <summary>
        /// 超时时长（ms），超过此时间的请求将取消
        /// </summary>
        public int Timeout { get; set; } = -1;

        /// <summary>
        /// 自定义对象, 可用于将请求时的一些细节传递到各类事件处理程序中使用
        /// </summary>
        public object CustomObject { get; set; }

        /// <summary>
        /// 获取自定义对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T? GetCustomObject<T>()
        {
            if (CustomObject == null)
            {
                return default;
            }

            try
            {
                return (T)CustomObject;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 获取自定义对象, 如果获取失败, 则返回给定的默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T? GetCustomObject<T>(T defaultValue)
        {
            return GetCustomObject<T>() ?? defaultValue;
        }
    }
}
