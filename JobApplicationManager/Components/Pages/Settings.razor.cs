// <copyright file="UserSettings.razor.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.

// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN 
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using JobApplicationManager.Components.Models;
using JobApplicationManager.Infrastructure.Data.Models;
using JobApplicationManager.Infrastructure.Data.Repositories;
using JobApplicationManager.Resources.Localize;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Syncfusion.Blazor.Inputs;

namespace JobApplicationManager.Components.Pages;

/// <summary>
/// Class Settings.
/// Implements the <see cref="ComponentBase" />
/// </summary>
/// <seealso cref="ComponentBase" />
public partial class Settings : ComponentBase
{
    [Inject]
    private IUserRepository _repository { get; set; } = default!;

    /// <summary>
    /// Gets or sets the localizer.
    /// </summary>
    /// <value>The localizer.</value>
    [Inject]
    private IStringLocalizer<SharedResource> _localizer { get; set; } = default!;

    /// <summary>
    /// Gets or sets the logger.
    /// </summary>
    /// <value>The logger.</value>
    [Inject]
    private ILogger<Settings> _logger { get; set; } = default!;

    protected SettingsModel UserSettings { get; set; } = new SettingsModel()
    {
        Firstname = "Max",
        Surname = "Mustermann",
        Street = "Musterstraße 1",
        City = "Musterstadt",
        Postcode = "12345",
        Phone = "0123456789",
        Email = "max.mustermann@example.com",
        Homepage = "https://www.example.com",
        BitlyApiKey = "", // No default value for Bitly API Key, it triggers a Warning.
        LatexPath = "/usr/local/texlive/2023/bin/x86_64-linux",
        SmtpServer = "smtp.example.com",
        SmtpUser = "your_smtp_user",
        SmtpPass = "your_smtp_password",
        SmtpPort = 587,
        SmtpServerOption = "Auto"
    };

    /// <summary>
    /// On initialized as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
    }

    /// <summary>
    /// Sets the user settings.
    /// </summary>
    protected async Task SetUserSettings()
    {
        var model = new User
        {
            Firstname = UserSettings.Firstname,
            Surname = UserSettings.Surname,
            Street = UserSettings.Street,
            City = UserSettings.City,
            Postcode = UserSettings.Postcode,
            Phone = UserSettings.Phone,
            Email = UserSettings.Email,
            Homepage = UserSettings.Homepage,
            BitlyApiKey = UserSettings.BitlyApiKey,
            LatexPath = UserSettings.LatexPath,
            SmtpServer = UserSettings.SmtpServer,
            SmtpUser = UserSettings.SmtpUser,
            SmtpPass = UserSettings.SmtpPass,
            SmtpPort = UserSettings.SmtpPort,
            SmtpServerOption = UserSettings.SmtpServerOption,
        };
        await _repository.AddAsync(model);
        _logger.LogInformation("Found new User data. Added it to Database");
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
}