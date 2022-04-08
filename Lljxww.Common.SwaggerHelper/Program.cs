using Lljxww.Common.SwaggerHelper;

string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample_data.json");
string jsonText = File.ReadAllText(path);

SwaggerModel? swaggerModel = SwaggerJsonHelper.GetSwaggerModel(jsonText);

Console.WriteLine(swaggerModel?.Version);