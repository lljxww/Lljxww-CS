using System.Text.Json.Nodes;
using Lljxww.ApiCaller;
using Lljxww.ApiCaller.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test;

[TestClass]
public class ApiResultTest
{
    private readonly IServiceCollection _services = new ServiceCollection();

    public ApiResultTest()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("apicaller.json")
            .Build();

        _services.ConfigureCaller(config);
    }

    [TestMethod]
    public void InvokeTest()
    {
        Caller? caller = _services.BuildServiceProvider().GetRequiredService<Caller>();

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
        Caller? caller = _services.BuildServiceProvider().GetRequiredService<Caller>();

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

    [TestMethod]
    public void IndexerTest()
    {
        string raw = "{\"BaseInfo\":{\"UserID\":501835086,\"UserName\":\"lljxww_02\"," +
                     "\"UserTypeID\":3,\"UserTypeName\":\"一卡通用户\",\"UserStatus\":1," +
                     "\"Password\":\"\",\"SuperPassword\":\"\",\"TacticGroupID\":0," +
                     "\"UserSource\":1,\"ParentID\":-1,\"ParentName\":\"\",\"RoamFlag" +
                     "\":0,\"IsInstitutiona\":0,\"DKFXVer\":\"\",\"ServiceType\":-1," +
                     "\"IsAutoLogin\":0,\"RoamChangeTime\":\"0001-01-01T00:00:00\"," +
                     "\"SkipRoamCertification\":false},\"ComInfo\":{\"RealName\":\"\"," +
                     "\"Area\":\"北京\",\"Email\":\"\",\"IDCard\":\"\",\"Profession\":" +
                     "\"\",\"Age\":\"\",\"Sex\":\"男\",\"Education\":\"\",\"UnitName\"" +
                     ":\"\",\"Telephone\":\"\",\"Mobile\":\"\",\"Address\":\"\",\"ZipCode" +
                     "\":\"\",\"RegisterTime\":\"2022-06-16T15:19:03\",\"Fax\":\"\"," +
                     "\"Major\":\"\",\"Interest\":\"\",\"Question\":\"\",\"Answer\":" +
                     "\"\",\"Email2\":\"\",\"BirthDay\":\"\",\"RegisterFrom\":\"\"," +
                     "\"CreateUser\":null,\"StartSchoolYear\":null,\"ResearchAreas" +
                     "\":null,\"ProfessionalTitle\":null,\"NickName\":null,\"Photo\":null," +
                     "\"Profile\":null,\"QQ\":null,\"Weixin\":null},\"CtrlInfo\":{\"ConnNumType" +
                     "\":0,\"IsConnNumFlag\":0,\"MaxConnCount\":0,\"IsSubUserFlag\":0," +
                     "\"MaxSubUserCount\":0,\"IsIPFlag\":0,\"IsFeeFlag\":1,\"IsShowMoney" +
                     "\":0.00,\"ShowCard\":0,\"IsProductLimitFlag\":0,\"IsEffectDateFlag" +
                     "\":0,\"StartTime\":\"2022-06-16T00:00:00\",\"EndTime\":\"2072-06-16T00:00:00" +
                     "\",\"IsDownloadFlag\":1,\"IsSingleLoginFlag\":0,\"IsUPLFlag\":1,\"IsCellFlag" +
                     "\":0,\"IsLoginIPNumFlag\":0,\"IsDownloadNumFlag\":0,\"IsRoamFlag\":0,\"MaxRoamCount" +
                     "\":0,\"MaxDownPageNum\":0,\"MaxDownPageNumDay\":0,\"MaxDownPageNumMonth\":0," +
                     "\"MaxDownPageNumYear\":0,\"MaxIPCountPerday\":0,\"MaxLogonIPPerDay\":0," +
                     "\"MaxDownTimesNum\":0,\"MaxDownTimesNumDay\":0,\"MaxDownTimesNumMonth\":0," +
                     "\"MaxDownTimesNumYear\":0,\"MaxRoamDownTimes\":0,\"MaxRoamDownTimesDay\":0," +
                     "\"MaxRoamDownTimesMonth\":0,\"MaxRoamDownTimesYear\":0,\"MaxCellMaxCount\":0," +
                     "\"SubUserNum\":0,\"PackPolicys\":[],\"IsStudent\":false,\"StudentEndTime\":" +
                     "\"0001-01-01T00:00:00\",\"MaxBalance\":0,\"MaxBalanceDay\":0,\"MaxBalanceMonth" +
                     "\":0,\"MaxBalanceYear\":0},\"IPInfo\":null,\"IPv6Info\":null,\"RightInfo\":{" +
                     "\"acqd\":null,\"capj\":null,\"cjfd\":null,\"cjfq\":null,\"cpfd\":null,\"zyzj" +
                     "\":null,\"zyzr\":null,\"测试代码\":null},\"ProductDownLimitInfo\":null,\"FreeRightInfo" +
                     "\":null,\"UserBindConfigInfoS\":null,\"UserBindConfigInfo\":null,\"DealerInfo" +
                     "\":null,\"IPAreaType\":null,\"ExtendInfo\":null,\"IPAreaLimit\":null,\"ClaimFiles" +
                     "\":[],\"LoadTime\":\"2022-06-16T15:20:34.4268906+08:00\"}";

        var result = new ApiResult(raw);
        var part = result["BaseInfo"];
        var partObj = JsonNode.Parse(part!);
        Assert.AreNotEqual("lljxww_02", partObj!["username"]);
    }
}