using Lljxww.ApiCaller;
using Lljxww.ApiCaller.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Lljxww.Test.ApiCaller;

[TestClass]
public class ApiCallerTest
{
    private readonly IServiceCollection _services = new ServiceCollection();

    public ApiCallerTest()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("./ApiCaller/apicaller.json")
            .Build();

        _services.ConfigureCaller(config);

        CallerEvents.OnExecuted += context =>
        {
            string apiName = context.ApiName;
        };
    }

    [TestMethod]
    public void InvokeTest()
    {
        Caller caller = _services.BuildServiceProvider().GetRequiredService<Caller>();

        ApiResult? result = caller.InvokeAsync("weibo.hot", requestOption: new RequestOption
        {
            CustomAuthorizeInfo = "123"
        }).Result;

        Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage!.StatusCode);
        Assert.AreEqual("1", result["ok"]);

        ApiResult? weatherResult = caller.InvokeAsync("weather.forecast", new
        {
            latitude = 39.54,
            longitude = 116.23,
            hourly = "temperature_2m,relativehumidity_2m,windspeed_10m"
        }).Result;
        
        Assert.AreEqual(HttpStatusCode.OK, weatherResult.HttpResponseMessage.StatusCode);
    }
}