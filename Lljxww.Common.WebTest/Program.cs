using Lljxww.Common.ApiCaller;
using Lljxww.Common.WebApiCaller;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCaller();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Caller.OnExecuted += context =>
{
    var request = (HttpRequest)context.RequestOption.CustomObject;
};

app.UseAuthorization();

app.MapControllers();

app.Run();