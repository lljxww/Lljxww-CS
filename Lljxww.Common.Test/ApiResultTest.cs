using Lljxww.Common.ApiCaller;
using Lljxww.Common.ApiCaller.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Common.Test;

[TestClass]
public class ApiResultTest
{
    private readonly IServiceCollection services = new ServiceCollection();

    public ApiResultTest()
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
        Assert.AreEqual(username, result!["Login"]);
        Assert.AreEqual(username, result!["lOgin"]);
        Assert.AreEqual(username, result!["loGin"]);
        Assert.AreEqual(username, result!["logIn"]);
        Assert.AreEqual(username, result!["logiN"]);
    }

    [TestMethod]
    public void EventTest()
    {
        Caller? caller = services.BuildServiceProvider().GetRequiredService<Caller>();

        string? username = "liang1224";

        ApiResult? result = caller.InvokeAsync("gh.GetUserInfo", new
        {
            username
        }).Result;

        Assert.AreEqual(false, result.Success);
        Assert.AreEqual(-1, result.Code);
        Assert.AreEqual(null, result.Message);

        ApiResult.GetCode += (apiResult, func) => 42;
        ApiResult.GetMessage += (apiResult, func) => "some_message";
        ApiResult.GetSuccess += (apiResult, predicate) => true;

        Assert.AreEqual(true, result.Success);
        Assert.AreEqual(42, result.Code);
        Assert.AreEqual("some_message", result.Message);
    }
}