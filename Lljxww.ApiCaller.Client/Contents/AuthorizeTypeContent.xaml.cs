using Lljxww.ApiCaller.Client.Util;
using Lljxww.ApiCaller.Client.Models;

namespace Lljxww.ApiCaller.Client.Contents;

public partial class AuthorizeTypeContent : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(AuthorizeTypeContent),
            typeof(string),
            typeof(AuthorizeTypeContent),
            string.Empty);

    public static readonly BindableProperty ItemIdProperty =
        BindableProperty.Create(
            nameof(AuthorizeTypeContent),
            typeof(string),
            typeof(AuthorizeTypeContent),
            string.Empty);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string ItemId
    {
        get
        {
            return (string)GetValue(ItemIdProperty);
        }
        set
        {
            SetValue(ItemIdProperty, value);
            Load(value);
        }
    }

    public AuthorizeTypeContent()
	{
		InitializeComponent();
	}

    private void Load(string id)
    {
        var config = ConfigFileUtil.LoadConfigById(id);
        BindingContext = new ManagedAuthorizeType(config);
    }

    private void authorizeTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}
