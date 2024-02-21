using Lljxww.ApiCaller;
using Lljxww.ApiCaller.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test.ApiCaller;

[TestClass]
public class ExceptionTest
{
    private readonly IServiceCollection _services = new ServiceCollection();

    public ExceptionTest()
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

    [TestMethod]
    public void ItemNotFoundExceptionTest()
    {
        Caller caller = _services.BuildServiceProvider().GetRequiredService<Caller>();
        _ = Assert.ThrowsException<ItemNotFoundException>(async () => await caller.InvokeAsync("some-service.undefined-method"));
    }

    [TestMethod]
    public void UnknownAuthorizationExceptionTest()
    {
        Caller caller = _services.BuildServiceProvider().GetRequiredService<Caller>();
        _ = Assert.ThrowsException<UnknownAuthorizationException>(async () => await caller.InvokeAsync("ecsp.no-authorization"));
    }
}