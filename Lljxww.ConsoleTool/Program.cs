using Lljxww.ApiCaller;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();
services.ConfigureCaller();

var app = new CommandLineApplication();

app.HelpOption();

var subject = app.Option("-s|--subject <SUBJECT>", "The subject", CommandOptionType.SingleValue);
subject.DefaultValue = "world";

var repeat = app.Option<int>("-n|--count <N>", "Repeat", CommandOptionType.SingleValue);
repeat.DefaultValue = 1;

app.OnExecute(() =>
{
    for (var i = 0; i < repeat.ParsedValue; i++)
    {
        Console.WriteLine($"Hello {subject.Value()}!");
    }
});

app.Command("get", command =>
{

});

return app.Execute(args);