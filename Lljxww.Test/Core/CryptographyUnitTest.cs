using Lljxww.Utilities.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test.Core;

[TestClass]
public class CryptographyUnitTest
{
    [TestMethod]
    public void SHA1CalTestMethod()
    {
        string source = "liangjw";
        string target = "bc9a767ffd41098041cdec50424a5f4dae9c23b0";

        Assert.AreEqual(target, SHA1.Calculate(source));
    }
}