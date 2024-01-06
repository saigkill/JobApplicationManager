using MailKit.Security;

namespace JobApplicationManager.ViewModels;

public partial class SettingsViewModel(IFolderPicker folderPicker) : BaseViewModel
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
    public string? LatexPath { get; set; } = Preferences.Get("LatexPath", "");

    public ObservableCollection<EmailOptions> EmailOptions { get; set; } =
    [
        new EmailOptions(emailOption: "None", option: SecureSocketOptions.None, id: 0),
        new EmailOptions(emailOption: "Auto", option: SecureSocketOptions.Auto, id: 1),
        new EmailOptions(emailOption: "SSL on Connect", option: SecureSocketOptions.SslOnConnect, id: 2),
        new EmailOptions(emailOption: "Start TLS", option: SecureSocketOptions.StartTls, id: 3),
        new EmailOptions(emailOption: "Start TLS When Available", option: SecureSocketOptions.StartTlsWhenAvailable,
            id: 4),
    ];

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

    public void GetLatexPath()
    {
        // Using native implementation
        LatexPath = folderPicker.PickFolder().ToString();
    }
}
