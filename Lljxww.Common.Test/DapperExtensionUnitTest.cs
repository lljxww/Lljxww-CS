using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Dapper;
using Lljxww.Common.Dapper.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using ColumnAttribute = Lljxww.Common.Dapper.Extensions.ColumnAttribute;

namespace Lljxww.Common.Test
{
    [TestClass]
    public class DapperExtensionUnitTest
    {
        private IDbConnection GetConn()
        {
            var connStr = "server=localhost;uid=root;pwd=123456;database=gans;";
            return new MySqlConnection(connStr);
        }

        [TestMethod]
        public void Test()
        {
            ColumnHelper.SetMapper(typeof(DapperExtensionUnitTest).Assembly);

            using var conn = GetConn();
            var testModel = conn.QueryFirst<TestModel>("select * from role_action");
            Assert.AreEqual(9, testModel.ActionId);
        }

        [Table("role_action")]
        public class TestModel
        {
            [Key]
            public int Id { get; set; }

            [Column("role_id")]
            public int RoleId { get; set; }

            [Column("action_id")]
            public int ActionId { get; set; }
        }
    }
}