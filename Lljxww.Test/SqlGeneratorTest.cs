using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lljxww.Dapper.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test;

[TestClass]
public class SqlGeneratorTest
{
    [TestMethod]
    public void BuildQueryTest()
    {
        string? query = SqlGenerator.BuildQuery<TypeA>();

        string? update = SqlGenerator.BuildUpdate<TypeB>(new List<string>
        {
            nameof(TypeA.Address),
            nameof(TypeA.LastName)
        });
        string? update2 = SqlGenerator.BuildUpdate<TypeB>();
        Assert.ThrowsException<InvalidOperationException>(() => SqlGenerator.BuildUpdate<TypeA>());

        string? insert = SqlGenerator.BuildInsert<TypeA>();
        Assert.ThrowsException<InvalidOperationException>(() => SqlGenerator.BuildInsert<TypeA>(true));
        string? insert2 = SqlGenerator.BuildInsert<TypeB>(true);
        string? insert3 = SqlGenerator.BuildInsert<TypeB>();
    }
}

[Table("my_table_a")]
public class TypeA
{
    public int ID { get; set; }

    [LColumn("first_name")] public string FirstName { get; set; }

    [LColumn("last_name")] public string LastName { get; set; }

    public string Address { get; set; }
}

[Table("my_table_b")]
public class TypeB
{
    [Key] public int ID { get; set; }

    [LColumn("first_name")] public string FirstName { get; set; }

    [LColumn("last_name")] public string LastName { get; set; }

    public string Address { get; set; }
}