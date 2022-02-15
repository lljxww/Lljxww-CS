using Lljxww.Common.ApiCaller;
using Lljxww.Common.ApiCaller.Models;
using Lljxww.Common.WebApiCaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Common.Test
{
    [TestClass]
    public class ApiCallerTest
    {
        private readonly IServiceCollection services = new ServiceCollection();

        public ApiCallerTest()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("apicaller.json")
                .Build();

            services.ConfigureCaller(config);
        }

        [TestMethod]
        public void InvokeTest()
        {
            Caller? caller = services.BuildServiceProvider().GetRequiredService<Caller>();

            string? username = "liang1224";

            ApiResult? result = caller.InvokeAsync("gh.GetUserInfo", new
            {
                username
            }).Result;

            Assert.AreEqual(username, result!["login"]);
        }
    }
}
