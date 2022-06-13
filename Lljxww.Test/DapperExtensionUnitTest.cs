using Dapper;
using Lljxww.Dapper.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Lljxww.Test;

[TestClass]
public class DapperExtensionUnitTest
{
    private IDbConnection GetConn()
    {
        string? connStr = "server=localhost;uid=root;pwd=123456;database=gans;";
        return new MySqlConnection(connStr);
    }

    [TestMethod]
    public void Test()
    {
        ColumnHelper.SetMapper(typeof(DapperExtensionUnitTest).Assembly);

        using IDbConnection? conn = GetConn();
        TestModel? testModel = conn.QueryFirst<TestModel>("select * from role_action");
        Assert.AreEqual(9, testModel.ActionId);
    }

    [Table("role_action")]
    public class TestModel
    {
        [Key] public int Id { get; set; }

        [LColumn("role_id")]
        public int RoleId { get; set; }

        [LColumn("action_id")]
        public int ActionId { get; set; }
    }
}