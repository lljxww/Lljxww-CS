using Lljxww.ApiCaller.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lljxww.Test.ApiCaller;

[TestClass]
public class ApiResultConvertUnitTest
{
    [TestMethod]
    public void TryConvertTestMethod()
    {
        //string str = "{\"success\":true,\"code\":1,\"message\":null,\"content\":{\"fileName\":\"GHZH201102023\",\"name\":\"乌鲁木齐市大气污染总量控制和降低机制研究\",\"year\":\"2011\",\"term\":\"02\",\"pageCount\":\"6\",\"page\":\"120-125\",\"pageCode\":\"\",\"albumCode\":\"A;B;\",\"author\":\"魏毅;加旭辉;魏勇;\",\"translator\":\"\",\"editor\":\"\",\"subjectCode\":\"B027;\",\"source\":\"干旱区资源与环境\",\"pubDate\":\"2011-02-15\",\"discCode\":\"SCTA1101\",\"extendFields\":{\"页\":\"120-125\",\"专题代码\":\"B027;\",\"期\":\"02\",\"拼音刊名\":\"GHZH\",\"译者\":\"\",\"表名\":\"CJFD2011\",\"作者\":\"魏毅;加旭辉;魏勇;\",\"页数\":\"6\",\"年\":\"2011\",\"文件名\":\"GHZH201102023\",\"专辑代码\":\"A;B;\",\"来源\":\"干旱区资源与环境\",\"发表时间\":\"2011-02-15\",\"光盘号\":\"SCTA1101\",\"编者\":\"\",\"题名\":\"乌鲁木齐市大气污染总量控制和降低机制研究\",\"页码\":\"\"}}}";
        string str2 =
            "{\"RightTypes\":0,\"IsShowFeePage\":true,\"SelfError\":5056,\"ParentError\":5033,\"ParentName\":null,\"Ip\":null,\"SelfInfo\":{\"UserName\":\"lljxww\",\"IsStudent\":false,\"StudentEndTime\":\"0001-01-01 00:00:00\",\"Balance\":0.0,\"Ticket\":0.0,\"PriceType\":0,\"Price\":0.0,\"Discount\":0.0,\"Expenses\":0.0},\"CardRights\":[],\"FeeRights\":[]}";

        ApiResult result = new(str2);
        _ = result.TryConvert<Price2FromApi>();
    }

    public class Price2FromApi
    {
        [DataMember] public int RightTypes { get; set; }

        [DataMember] public bool IsShowFeePage { get; set; }

        [DataMember] public int SelfError { get; set; }

        [DataMember] public int ParentError { get; set; }

        [DataMember] public string ParentName { get; set; }

        [DataMember] public string Ip { get; set; }

        [DataMember] public SelfInfoModel SelfInfo { get; set; }

        [DataMember] public IList<CardRightsModel> CardRights { get; set; }

        [DataMember] public IList<FeeRightsModel> FeeRights { get; set; }

        [DataMember] public FileAuthModel FileAuthModel { get; set; }
    }

    [Serializable]
    [DataContract]
    public class FileAuthModel
    {
        /// <summary>
        /// 库代码
        /// </summary>
        [DataMember]
        public string DbCode { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [DataMember]
        public string FileId { get; set; }

        /// <summary>
        /// RootId
        /// </summary>
        [DataMember]
        public string RootId { get; set; }

        /// <summary>
        /// 查询权限需要的kbase字段名
        /// </summary>
        [DataMember]
        public IList<string> Fields { get; set; } =
        [
            "文件名",
            "题名",
            "年",
            "期",
            "页数",
            "页",
            "页码",
            "专辑代码",
            "作者",
            "译者",
            "编者",
            "专题代码",
            "来源",
            "发表时间",
            "光盘号"
        ];

        /// <summary>
        /// 从kbase中查询出来的字段名及其值
        /// </summary>
        [DataMember]
        public Dictionary<string, string> FieldValuePairs { get; set; } = [];
    }

    [Serializable]
    [DataContract]
    public class SelfInfoModel
    {
        [DataMember] public string UserName { get; set; }

        [DataMember] public bool IsStudent { get; set; }

        [DataMember] public string StudentEndTime { get; set; }

        [DataMember] public double Balance { get; set; }

        [DataMember] public double Ticket { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CardRightsModel
    {
        [DataMember] public RightUserBaseModel RightUserBase { get; set; }

        [DataMember] public string CardName { get; set; }

        [DataMember] public string Platform { get; set; }

        [DataMember] public string CardId { get; set; }

        [DataMember] public int RemainPage { get; set; }

        [DataMember] public DateTime? OverTime { get; set; }

        [DataMember] public int PurchasedPage { get; set; }
    }

    [Serializable]
    [DataContract]
    public class FeeRightsModel
    {
        [DataMember] public RightUserBaseModel RightUserBase { get; set; }

        [DataMember] public bool IsStudent { get; set; }

        [DataMember] public DateTime? StudentEndTime { get; set; }

        [DataMember] public int PriceType { get; set; }

        [DataMember] public double Price { get; set; }

        [DataMember] public double Discount { get; set; }

        [DataMember] public double Expenses { get; set; }
    }

    [Serializable]
    [DataContract]
    public class RightUserBaseModel
    {
        [DataMember] public string UserName { get; set; }

        [DataMember] public bool IsJfOrBk { get; set; }

        [DataMember] public bool IsBind { get; set; }

        [DataMember] public bool IsMirror { get; set; }
    }
}