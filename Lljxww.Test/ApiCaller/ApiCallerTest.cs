using Lljxww.ApiCaller;
using Lljxww.Utilities.Cryptography;
using Lljxww.Utilities.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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

        _ = _services.ConfigureCallerWithConfigFile(config);

        CallerEvents.OnExecuted += context =>
        {
            string apiName = context.ApiName;
        };
    }

    private static ApiResult GetJwt(Caller caller)
    {
        string timestamp = Timestamp.Get();
        string appId = "YOUR_ID";
        string appKey = "YOUR_SECRET";
        string sign = SHA1.Calculate($"timestamp={timestamp}&appid={appId}&appkey={appKey}");

        return caller.InvokeAsync("ecsp.get-jwt", new
        {
            timestamp,
            appId,
            sign,
            clientIp = "110.242.68.66"
        }).Result;
    }

    [TestMethod]
    public void GetJwtTest()
    {
        Caller caller = _services.BuildServiceProvider().GetRequiredService<Caller>();

        ApiResult result = GetJwt(caller);

        Assert.IsNotNull(result["Content"]);
        Assert.AreEqual(2, result["Content"]!.Count(c => c == '.'));
    }

    [TestMethod]
    public void ValidateJwtTest()
    {
        Caller caller = _services.BuildServiceProvider().GetRequiredService<Caller>();
        string? jwt = GetJwt(caller)["Content.JwtToken"];

        ApiResult? result = caller.InvokeAsync("ecsp.validate-jwt", new
        {
            jwt
        }).Result;

        Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage!.StatusCode);
        Assert.AreEqual("true", result["Content"]);
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
    }

    [TestMethod]
    public void InvokeTest2()
    {
        Caller caller = _services.BuildServiceProvider().GetRequiredService<Caller>();

        AuthenticationsStore.AddAuthFunc("ecsp-jwt", context =>
        {
            context.RequestMessage.Headers.Add("Authorization", $"Bearer {GetJwt(caller)}");
            return context;
        });

        ApiResult? result = caller.InvokeAsync("ecsp.timestamp").Result;

        Assert.AreEqual(HttpStatusCode.OK, result.HttpResponseMessage!.StatusCode);
        Assert.AreEqual(12, result["Content"]!.Length);
    }
}