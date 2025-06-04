// <copyright file="Settings.razor.cs" company="Sascha Manns">
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
using JobApplicationManager.Domain.Entities;
using JobApplicationManager.Domain.Interfaces;
using JobApplicationManager.Infrastructure.Models;
using JobApplicationManager.Resources.Localize;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Syncfusion.Blazor.Inputs;

namespace JobApplicationManager.Components.Pages
{
    /// <summary>
    /// Class Settings.
    /// Implements the <see cref="ComponentBase" />
    /// </summary>
    /// <seealso cref="ComponentBase" />
    public partial class Settings : ComponentBase
    {
        [Inject]
        private IUserRepository Repository { get; set; } = default!;

        [Inject]
        private IStringLocalizer<SharedResource> Localizer { get; set; } = default!;

        [Inject]
        private ILogger<Settings> Logger { get; set; } = default!;

        protected string SmtpServerOption { get; set; } = "Auto";

        protected List<PickerEmailOptionsModel> EmailOptions { get; set; } = new PickerEmailOptionsModel().GetOptions()!;

        protected SettingsModel UserSettings { get; set; } = new SettingsModel()
        {
            FirstName = "Max",
            Surname = "Example",
            Street = "Examplestreet 1",
            City = "Examplecity",
            Postcode = "12345",
            Phone = "0123456789",
            Email = "max.example@example.com",
            Homepage = "https://www.example.com",
            BitLyApiKey = "", // No default value for Bitly API Key, it triggers a CI/CD Warning.
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
        //protected override async Task OnInitializedAsync()
        //{
        //}

        /// <summary>
        /// Sets the user settings.
        /// </summary>
        protected async Task SetUserSettings()
        {
            var model = new User
            {
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
                SmtpServerOption = UserSettings.SmtpServerOption,
            };
            await Repository.AddAsync(model);
            Logger.LogInformation("Found new User data. Added it to Database");
            //var user = await GraphClient.Me.Request().GetAsync();
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
}