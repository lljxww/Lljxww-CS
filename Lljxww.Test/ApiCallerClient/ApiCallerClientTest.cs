//using Lljxww.ApiCaller;
//using Lljxww.ApiCaller.Client;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Lljxww.Test.ApiCallerClient;

//[TestClass]
//public class ApiCallerClientTest
//{
//    private readonly IServiceCollection _services = new ServiceCollection();

//    public ApiCallerClientTest()
//    {
//        IConfigurationRoot? config = new ConfigurationBuilder()
//            .AddJsonFile("./ApiCallerClient/apicaller.json")
//            .Build();

//        _services.ConfigureCaller(config);
//        _services.AddSingleton<CallerClient>();
//    }

//    [TestMethod]
//    public void Test()
//    {
//        ServiceProvider serviceProvider = _services.BuildServiceProvider();
//        var client = serviceProvider.GetRequiredService<CallerClient>();

//        var result = client.GhGetUserInfoAsync(new
//        {
//            username = "liang1224"
//        }).Result;

//        Assert.AreEqual("liang1224", result["login"]);
//    }
//}