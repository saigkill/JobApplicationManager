using JobApplicationManager.Components.Models;
using JobApplicationManager.Domain.Entities;
using JobApplicationManager.Domain.Interfaces;
using JobApplicationManager.Infrastructure.Models;
using JobApplicationManager.Resources;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Syncfusion.Blazor.Inputs;

namespace JobApplicationManager.Components.Pages;
public partial class Settings : ComponentBase
{
    [Inject]
    private IUserRepository Repository { get; set; } = default!;

    [Inject]
    private IUserEntraIdService EntraIdService { get; set; } = default!;

    [Inject]
    private IStringLocalizer<SfResources> Localizer { get; set; } = default!;

    [Inject]
    private ILogger<Settings> Logger { get; set; } = default!;

    protected string SmtpServerOption { get; set; } = string.Empty;

    protected List<PickerEmailOptionsModel> EmailOptions { get; set; } = new PickerEmailOptionsModel().GetOptions()!;

    protected SettingsModel UserSettings { get; set; } = new SettingsModel();

    /// <summary>
    /// On initialized as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        var user = await Repository.GetByGuidAsync(EntraIdService.GetUserId());
        if (user != null)
        {
            UserSettings.FirstName = user.Firstname;
            UserSettings.Surname = user.Surname;
            UserSettings.Street = user.Street;
            UserSettings.City = user.City;
            UserSettings.Postcode = user.Postcode;
            UserSettings.Phone = user.Phone;
            UserSettings.Email = user.Email;
            UserSettings.Homepage = user.Homepage;
            UserSettings.BitLyApiKey = user.BitlyApiKey;
            UserSettings.LatexPath = user.LatexPath;
            UserSettings.SmtpServer = user.SmtpServer;
            UserSettings.SmtpUser = user.SmtpUser;
            UserSettings.SmtpPass = user.SmtpPass;
            UserSettings.SmtpPort = user.SmtpPort;
        }
        else
        {
            UserSettings.FirstName = "Max";
            UserSettings.Surname = "Example";
            UserSettings.Street = "Examplestreet 1";
            UserSettings.City = "Examplecity";
            UserSettings.Postcode = "12345";
            UserSettings.Phone = "0123456789";
            UserSettings.Email = "max.example@example.com";
            UserSettings.Homepage = "https://www.example.com";
            UserSettings.BitLyApiKey = ""; // No default value for Bitly API Key, it triggers a CI/CD Warning.
            UserSettings.LatexPath = "/usr/local/texlive/2023/bin/x86_64-linux";
            UserSettings.SmtpServer = "smtp.example.com";
            UserSettings.SmtpUser = "your_smtp_user";
            UserSettings.SmtpPass = "your_smtp_password";
            UserSettings.SmtpPort = 587;
            SmtpServerOption = "Auto";
        }
    }

    /// <summary>
    /// Sets the user settings.
    /// </summary>
    protected async Task SetUserSettings()
    {
        var user = await Repository.GetByGuidAsync(EntraIdService.GetUserId());

        var model = new User
        {
            UserGuid = EntraIdService.GetUserId()!,
            Firstname = UserSettings.FirstName,
            Surname = UserSettings.Surname,
            Street = UserSettings.Street,
            City = UserSettings.City,
            Postcode = UserSettings.Postcode,
            Phone = UserSettings.Phone,
            Email = UserSettings.Email,
            Homepage = UserSettings.Homepage,
            BitlyApiKey = UserSettings.BitLyApiKey,
            LatexPath = UserSettings.LatexPath,
            SmtpServer = UserSettings.SmtpServer,
            SmtpUser = UserSettings.SmtpUser,
            SmtpPass = UserSettings.SmtpPass,
            SmtpPort = UserSettings.SmtpPort,
        };

        if (user != null)
        {
            await Repository.UpdateAsync(model);
        }
        else
        {
            await Repository.AddAsync(model);
        }

        Logger.LogInformation("User data has been updated or added successfully.");
    }

    /// <summary>
    /// Handles the <see cref="E:Change" /> event.
    /// </summary>
    /// <param name="args">The <see cref="UploadChangeEventArgs"/> instance containing the event data.</param>
    private async Task OnChange(UploadChangeEventArgs args)
    {
        try
        {
            foreach (var file in args.Files)
            {
                var path = "" + file.FileInfo.Name;

                // The FileUploader component is set for single file upload, so we can directly access the first file.
                UserSettings.LatexPath = path;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void OnClearHandler(ClearingEventArgs obj)
    {
        UserSettings.LatexPath = string.Empty;
    }
}
