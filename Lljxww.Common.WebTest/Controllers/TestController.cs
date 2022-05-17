using Lljxww.Common.ApiCaller.Models;
using Lljxww.Common.WebApiCaller;
using Microsoft.AspNetCore.Mvc;

namespace Lljxww.Common.WebTest.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly Caller _caller;

    public TestController(Caller caller)
    {
        _caller = caller;
    }

    [HttpGet(Name = "custom-object")]
    public async Task<string> GetAsync()
    {
        var result = await _caller.InvokeAsync("gh.GetUserInfo", new
        {
            username = "liang1224"
        }, new RequestOption
        {
            CustomObject = Request
        });

        return result.RawStr;
    }
}