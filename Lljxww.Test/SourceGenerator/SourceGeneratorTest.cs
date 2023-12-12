using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lljxww.SourceGenerator;

namespace Lljxww.Test.SourceGenerator;

[TestClass]
public class SourceGeneratorTest
{
    [EnumExtensions]
    public enum TestEnum
    {
        BN,
        YH,
        ZP,
        JH,
        LJ,
        HJ,
        LM,
        WY,
        HY,
    }

    [TestMethod]
    public void GeneratedCodeTest()
    {
        Assert.AreEqual("HY", TestEnum.HY.ToStringFast());
    }
}
