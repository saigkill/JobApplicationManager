namespace JobApplicationManager.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    public string UserFirstname { get; set; } = Preferences.Get("UserFirstName", "");
    public string UserSurname { get; set; } = Preferences.Get("UserSurname", "");
    public string UserStreet { get; set; } = Preferences.Get("UserStret", "");
    public string UserCity { get; set; } = Preferences.Get("UserCity", "");
    public string UserZip { get; set; } = Preferences.Get("UserZip", "");
    public string UserPhone { get; set; } = Preferences.Get("UserPhone", "");
    public string UserEmail { get; set; } = Preferences.Get("UserEmail", "");
    public string UserHomepage { get; set; } = Preferences.Get("UserHomepage", "");
    public string ExternalBitLyToken { get; set; } = Preferences.Get("ExternalBitLyToken", "");
    public string EmailServer { get; set; } = Preferences.Get("EmailSmtpServer", "");
    public string EmailUser { get; set; } = Preferences.Get("EmailUser", "");
    public string EmailPass { get; set; } = Preferences.Get("EmailPassword", "");
    public string EmailPort { get; set; } = Preferences.Get("EmailSmtpPort", "");
    private string EmailServerOptions { get; set; } = Preferences.Get("EmailServerOption", "");
    public string LatexPath { get; set; } = Preferences.Get("LatexPath", "");

    public SettingsViewModel()
    {
        //(Preferences.Clear();
    }

    public void SaveSettings()
    {
        Preferences.Set("UserFirstName", UserFirstname);
        Preferences.Set("UserSurname", UserSurname);
        Preferences.Set("UserStreet", UserStreet);
        Preferences.Set("UserCity", UserCity);
        Preferences.Set("UserZip", UserZip);
        Preferences.Set("UserPhone", UserPhone);
        Preferences.Set("UserEmail", UserEmail);
        Preferences.Set("UserHomepage", UserHomepage);
        Preferences.Set("ExternalBitLyToken", ExternalBitLyToken);
        Preferences.Set("EmailSmtpServer", EmailServer);
        Preferences.Set("EmailUser", EmailUser);
        Preferences.Set("EmailPassword", EmailPass);
        Preferences.Set("EmailSmtpPort", EmailPort);
        Preferences.Set("LatexPath", LatexPath);
        Preferences.Set("EmailServerOption", EmailServerOptions);
    }

    public void picker_SelectionChanged()
    {
        EmailServerOptions = DataSource.ToString();
    }

    public void picker_OkButtonClicked()
    {
        Preferences.Set("EmailServerOption", EmailServerOptions);
    }

    private ObservableCollection<object> dataSource = new ObservableCollection<object>()
    {
        "StartTls", "Auto", "None", "SslOnConnect", "StartTlsWhenAvailable"
    };

    public ObservableCollection<object> DataSource
    {
        get
        {
            return dataSource;
        }
        set
        {
            dataSource = value;
        }
    }
}
