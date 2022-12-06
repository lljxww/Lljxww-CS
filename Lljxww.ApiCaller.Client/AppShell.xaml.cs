using Lljxww.ApiCaller.Client.Views;

namespace Lljxww.ApiCaller.Client;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ApiCallerConfigPage), typeof(ApiCallerConfigPage));
    }
}