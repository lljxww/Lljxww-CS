using Lljxww.ApiCaller;
using Lljxww.ApiCaller.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test.ApiCaller;

[TestClass]
public class ApiCallerResultInitTest
{
    private readonly IServiceCollection services = new ServiceCollection();

    public ApiCallerResultInitTest()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("./ApiCaller/apicaller.json")
            .Build();

        _ = services.ConfigureCaller(config);
    }

    [TestMethod]
    public void InitTest()
    {
        string jsonStr =
            "{\"Success\":true,\"Code\":1,\"Message\":\"\",\"IdenId\":\"WEEvREdCZHpVUUwwUHJpblVNWGJFd2h6bmtVQVJHbUQ5ZU5CVEpnTms2SG5SQW4v$AiWoHpiIFekDNOPcgc2OGo3vKIcpcMmDwlXisKWvbvx7tmezVSp0eA!!\",\"Username\":\"lljxww\",\"PersonUserName\":\"lljxww\",\"InstUserName\":\"\",\"InstShowName\":\"\"}";

        ApiResult result = new(jsonStr);

        Assert.IsTrue(result.Success);
        Assert.AreEqual("1", result["code"]);
        Assert.AreEqual(1, result.Code);
    }
}