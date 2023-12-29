using System.Diagnostics.CodeAnalysis;

using Microsoft.EntityFrameworkCore;

namespace JobApplicationManager;

[SuppressMessage("ReSharper", "MethodTooLong")]
public static class MauiProgram
{
    private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
                fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
                fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<SettingsPage>();

        builder.Services.AddTransient<SampleDataService>();
        builder.Services.AddTransient<ListDetailDetailViewModel>();
        builder.Services.AddTransient<ListDetailDetailPage>();

        builder.Services.AddSingleton<ListDetailViewModel>();
        builder.Services.AddSingleton<ListDetailPage>();

        builder.Services.AddSingleton<LocalizationViewModel>();
        builder.Services.AddSingleton<LocalizationPage>();

#if WINDOWS
        builder.Services.AddTransient<IFolderPicker, Platforms.Windows.FolderPicker>();
#elif MACCATALYST
		builder.Services.AddTransient<IFolderPicker, Platforms.MacCatalyst.FolderPicker>();
#endif
        builder.Services.AddSingleton<ICsvParserService, CsvParserService>();
        builder.Services.AddSingleton<IEmailService, EmailService>();

        builder.Services.AddDbContext<Context.JobApplicationManager>(options =>
            options.UseSqlite(Path.Combine("Data Source=", AppDataPath, "JobApplicationManager", "JobApplicationManager.sqlite3;Cache=Shared")));

        string? windowsSecret = builder.Configuration["AppCenter:WindowsDesktop"];
        string? macosSecret = builder.Configuration["AppCenter:MacOS"];
        AppCenter.Start(
            $"windowsdesktop={windowsSecret}" +
            $"macos={macosSecret}",
            typeof(Analytics), typeof(Crashes));

        return builder.Build();
    }
}
