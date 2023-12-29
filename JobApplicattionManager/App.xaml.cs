using JobApplicationManager;

namespace JobApplicattionManager;

public partial class App : Application
{
    public App()
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");
        InitializeComponent();

        // Uncomment the following as a quick way to test loading resources for different languages
        // CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo("es");

        string firstRun = Preferences.Get("FirstRun", "");
        if (string.IsNullOrEmpty(firstRun))
        {
            SetupJam.CheckDocumentsPath();
            SetupJam.CheckTmpPath();
            Preferences.Set("FirstRun", "false");
            MainPage = new AppShell();
        }
        else
        {
            MainPage = new AppShell();
        }
    }
}
