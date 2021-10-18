using Lljxww.Common.Utilities.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Lljxww.Common.Test
{
    [TestClass]
    public class AttributeExtensionUnitTest
    {
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class MyAttribute : Attribute
        {
            public string Info { get; set; }

            public MyAttribute(string info)
            {
                Info = info;
            }
        }

        public enum MyEnum
        {
            [My("学生")]
            Student,

            [My("教师")]
            Teacher
        }

        [TestMethod]
        public void TestMethod1()
        {
            MyAttribute attr = MyEnum.Student.GetInfo<MyEnum, MyAttribute>(AttributeTargets.Field, nameof(MyEnum.Student));
            Assert.AreEqual("学生", attr.Info);
        }

        /// <summary>
        /// 担保方式
        /// </summary>
        public enum GuaraanteeModeType
        {
            [EnumInfo(0, "信用")]
            Credit,

            [EnumInfo(1, "保证担保")]
            Guarantee,

            [EnumInfo(2, "抵押担保")]
            Mortgage,

            [EnumInfo(3, "质押担保")]
            Pledge
        }

        [AttributeUsage(AttributeTargets.Field)]
        public class EnumInfoAttribute : Attribute
        {
            public string Info { get; set; }

            public int ID { get; set; }

            public EnumInfoAttribute(int id, string info)
            {
                ID = id;
                Info = info;
            }
        }


        [TestMethod]
        public void TestMethod2()
        {
            GuaraanteeModeType item = GuaraanteeModeType.Credit;
            EnumInfoAttribute attr = item.GetInfo<GuaraanteeModeType, EnumInfoAttribute>(AttributeTargets.Field, nameof(item));
            Assert.AreEqual("信用", attr.Info);
        }
    }
}
