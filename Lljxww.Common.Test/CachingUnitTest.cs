using System;
using System.Threading;
using System.Threading.Tasks;
using Lljxww.Common.CSRedis.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Common.Test
{
    [TestClass]
    public class CachingUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            ServiceProvider services = new ServiceCollection()
                .ConfigureCache("sso.dev.cnki.net,defaultDatabase=3")
                .BuildServiceProvider();

            Caching caching = services.GetRequiredService<Caching>();

            string value = Guid.NewGuid().ToString();
            caching.Set("test1", value);

            Assert.AreEqual(value, caching.Get<string>("test1"));

            string? a = caching.Invoke("test-lock", () =>
            {
                return Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    return "lock-result";
                }).Result;
            });

            Assert.AreEqual("lock-result", a);
        }
    }
}
