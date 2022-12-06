using Lljxww.ApiCaller.Client.Models;
using Lljxww.ApiCaller.Client.Util;

namespace Lljxww.ApiCaller.Client.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class ApiCallerConfigPage : ContentPage
{
	public string ItemId
	{
		set => LoadConfig(value);
	}

	public ApiCallerConfigPage()
	{
		InitializeComponent();
	}

	private void LoadConfig(string itemId)
	{
		ManagedApiCallerConfig configModel = ConfigFileUtil.LoadConfigById(itemId);
		BindingContext = configModel;
	}

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
		if (BindingContext is ManagedApiCallerConfig config)
		{
			if (config != null)
			{
				config.Name = TextEditor.Text;
			}
		}
		else
		{
			config = new ManagedApiCallerConfig
            {
                Id = Guid.NewGuid().ToString(),
                Name = TextEditor.Text
            };
        }

        ConfigFileUtil.SaveConfigFile(config);

        await Shell.Current.GoToAsync("..");
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is ManagedApiCallerConfig config)
        {
			ConfigFileUtil.DeleteConfigFile(config.Id);
        }

        await Shell.Current.GoToAsync("..");
    }
}
