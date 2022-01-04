using System.Net.Http;

namespace Lljxww.Common.WebApiCaller
{
    internal static class HttpClientInstance
    {
        private static readonly HttpClient client;

        static HttpClientInstance()
        {
            client = new HttpClient();

            client.DefaultRequestHeaders.Add("User-Agent", "Lljxww.WebApiCaller");
            client.DefaultRequestHeaders.Connection.Add("keep-alive");
        }

        public static HttpClient Get()
        {
            return client;
        }
    }
}
