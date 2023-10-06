using System;
using Lljxww.ApiCaller.Models.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test;

[TestClass]
public class Test
{
    [TestMethod]
    public void PropertyTest()
    {
        var props = typeof(ApiCallerConfig).GetProperties();
        foreach (var prop in props)
        {
            var a = prop.DeclaringType;
        }
    }
}
