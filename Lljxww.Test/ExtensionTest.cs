using System.Collections.Generic;
using Lljxww.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test;

[TestClass]
public class ExtensionTest
{
    [TestMethod]
    public void Test()
    {
        var obj = new
        {
            A = 1,
            B = 2,
            C = 3,
            D = default(string),
            E = default(object)
        };

        Dictionary<string, string>? dic = obj.ToDictionaryWithoutNullProperties();

        Assert.ThrowsException<KeyNotFoundException>(() =>
        {
            string? a = dic["D"];
        });

        Assert.AreEqual("1", dic["A"]);
    }
}