namespace Lljxww.Common.SwaggerHelper
{
    internal class MethodContext
    {
        /// <summary>
        /// Http方法
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public Type[]? Parameters { get; set; }
    }
}
