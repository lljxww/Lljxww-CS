using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Lljxww.Common.Test
{
    [TestClass]
    public class JsonNodeTest
    {
        [TestMethod]
        public void JsonNodeInitTest()
        {
            string? jsonStr = JsonSerializer.Serialize(TestModelGen(otherModelCount: 5));

            JsonNode jsonNode = JsonNode.Parse(jsonStr, new JsonNodeOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

            JsonObject jsonObject = jsonNode.AsObject();
            JsonArray jsonArray = jsonNode["othermodels"]!.AsArray();
            JsonValue jsonValue = jsonNode["name"]!.AsValue();

            JsonDocument? jsonDoc = JsonDocument.Parse(jsonStr);

            IList<Dictionary<string, JsonNode>>? dic = JsonSerializer.Deserialize<IList<Dictionary<string, JsonNode>>>(jsonArray);

            Console.ReadKey();
        }

        public class TestModel
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public IList<string> Books { get; set; }

            public IList<TestModel> OtherModels { get; set; }
        }

        public TestModel TestModelGen(int bookCount = 5, int otherModelCount = 0)
        {
            Random random = new();

            List<string>? books = new();
            for (int i = 0; i < bookCount; i++)
            {
                books.Add(Guid.NewGuid().ToString().Replace("-", ""));
            }

            List<TestModel>? otherModels = new();
            for (int j = 0; j < otherModelCount; j++)
            {
                otherModels.Add(TestModelGen());
            }

            return new TestModel
            {
                Id = random.Next(10000),
                Name = Guid.NewGuid().ToString().Replace("-", ""),
                Books = books,
                OtherModels = otherModels
            };
        }
    }
}