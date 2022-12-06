namespace Lljxww.ApiCaller.Client.Views;

public partial class AllApiCallerConfigPage : ContentPage
{
	public AllApiCallerConfigPage()
	{
		InitializeComponent();

		BindingContext = new Models.CallerConfig();
	}

    protected override void OnAppearing()
    {
		((Models.CallerConfig)BindingContext).LoadConfigs();
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ApiCallerConfigPage));
    }

    private async void allApiCallerConfigs_SelectionChanged(Object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var config = (Models.ManagedApiCallerConfig)e.CurrentSelection[0];
            await Shell.Current.GoToAsync($"{nameof(ApiCallerConfigPage)}?{nameof(ApiCallerConfigPage.ItemId)}={config.Id}");
        }
    }
}
