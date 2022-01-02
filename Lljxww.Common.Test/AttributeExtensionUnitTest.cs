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
            [My("ѧ��")]
            Student,

            [My("��ʦ")]
            Teacher
        }

        [TestMethod]
        public void TestMethod1()
        {
            MyAttribute? attr = MyEnum.Student.GetInfo<MyEnum, MyAttribute>(AttributeTargets.Field, nameof(MyEnum.Student));
            Assert.AreEqual("ѧ��", attr.Info);
        }

        /// <summary>
        /// ������ʽ
        /// </summary>
        public enum GuaraanteeModeType
        {
            [EnumInfo(0, "����")]
            Credit,

            [EnumInfo(1, "��֤����")]
            Guarantee,

            [EnumInfo(2, "��Ѻ����")]
            Mortgage,

            [EnumInfo(3, "��Ѻ����")]
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
            Assert.AreEqual("����", attr.Info);
        }
    }
}
