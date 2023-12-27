using Lljxww.SourceGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
