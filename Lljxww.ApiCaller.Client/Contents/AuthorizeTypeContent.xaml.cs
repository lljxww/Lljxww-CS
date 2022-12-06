namespace Lljxww.ApiCaller.Client.Contents;

public partial class AuthorizeTypeContent : ContentView
{
    public static readonly BindableProperty TitleProperty =
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

    public AuthorizeTypeContent()
	{
		InitializeComponent();
	}
}
