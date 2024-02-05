using Lljxww.ApiCaller.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test.ConsoleTool;

[TestClass]
public class Test
{
    [TestMethod]
    public void PropertyTest()
    {
        System.Reflection.PropertyInfo[] props = typeof(ApiCallerConfig).GetProperties();
        foreach (System.Reflection.PropertyInfo prop in props)
        {
            _ = prop.DeclaringType;
        }
    }
}
