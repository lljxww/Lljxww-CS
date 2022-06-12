using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lljxww.Utilities.SqlHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lljxww.Test;

[TestClass]
public class SqlGeneratorTest
{
    [TestMethod]
    public void BuildQueryTest()
    {
        var query = SqlGenerator.BuildQuery<TypeA>();
        
        var update = SqlGenerator.BuildUpdate<TypeB>(new List<string>
        {
            nameof(TypeA.Address),
            nameof(TypeA.LastName)
        });
        var update2 = SqlGenerator.BuildUpdate<TypeB>();
        Assert.ThrowsException<InvalidOperationException>(() => SqlGenerator.BuildUpdate<TypeA>());
        
        var insert = SqlGenerator.BuildInsert<TypeA>();
        Assert.ThrowsException<InvalidOperationException>(() => SqlGenerator.BuildInsert<TypeA>(true));
        var insert2 = SqlGenerator.BuildInsert<TypeB>(true);
        var insert3 = SqlGenerator.BuildInsert<TypeB>();
    }
}

[Table("my_table_a")]
public class TypeA
{
    public int ID { get; set; }
    
    [Column("first_name")]
    public string FirstName { get; set; }
    
    [Column("last_name")]
    public string LastName { get; set; }
    
    public string Address { get; set; }
}

[Table("my_table_b")]
public class TypeB
{
    [Key]
    public int ID { get; set; }
    
    [Column("first_name")]
    public string FirstName { get; set; }
    
    [Column("last_name")]
    public string LastName { get; set; }
    
    public string Address { get; set; }
}