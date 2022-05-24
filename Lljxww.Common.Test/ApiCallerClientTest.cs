using Lljxww.Common.ApiCaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Common.Test;

[TestClass]
public class ApiCallerClientTest
{
    private readonly IServiceCollection services = new ServiceCollection();

    public ApiCallerClientTest()
    {
        IConfigurationRoot? config = new ConfigurationBuilder()
            .AddJsonFile("apicaller.json")
            .Build();

        services.ConfigureCaller(config);
        //services.AddSingleton<CallerClient>();
    }

    [TestMethod]
    public void Test()
    {
        ServiceProvider? serviceProvider = services.BuildServiceProvider();
        //var client = serviceProvider.GetRequiredService<CallerClient>();

        //var result = client.GhGetUserInfoAsync(new
        //{
        //    username = "liang1224"
        //}).Result;

        //Assert.AreEqual("liang1224", result["login"]);
    }
}