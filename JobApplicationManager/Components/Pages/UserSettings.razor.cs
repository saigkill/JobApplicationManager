// <copyright file="UserSettings.razor.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.

namespace JobApplicationManager.Components.Pages
{
    using JobApplicationManager.Data.Models;
    using JobApplicationManager.Data.Repositories;

    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// Class UserSettings.
    /// Implements the <see cref="ComponentBase" />
    /// </summary>
    /// <seealso cref="ComponentBase" />
    public partial class UserSettings : ComponentBase
    {
        /// <summary>
        /// Gets or sets the localizer.
        /// </summary>
        /// <value>The localizer.</value>
        [Inject]
        private IStringLocalizer<UserSettings> _loc { get; set; }

        /// <summary>
        /// Gets or sets users Firstname.
        /// </summary>
        /// <value>The Firstname.</value>
        private string Firstname { get; set; }

        /// <summary>
        /// Gets or sets users surname.
        /// </summary>
        /// <value>The surname.</value>
        private string Surname { get; set; }

        /// <summary>
        /// Gets or sets users street.
        /// </summary>
        /// <value>The street.</value>
        private string Street { get; set; }

        /// <summary>
        /// Gets or sets users city.
        /// </summary>
        /// <value>The city.</value>
        private string City { get; set; }

        /// <summary>
        /// Gets or sets users postcode.
        /// </summary>
        /// <value>The postcode.</value>
        private string Postcode { get; set; }

        /// <summary>
        /// Gets or sets users phone.
        /// </summary>
        /// <value>The phone.</value>
        private string Phone { get; set; }

        /// <summary>
        /// Gets or sets users email.
        /// </summary>
        /// <value>The email.</value>
        private string Email { get; set; }

        /// <summary>
        /// Gets or sets users homepage.
        /// </summary>
        /// <value>The homepage.</value>
        private string? Homepage { get; set; }

        /// <summary>
        /// Gets or sets users bitly API key.
        /// </summary>
        /// <value>The bitly API key.</value>
        private string? BitlyApiKey { get; set; }

        /// <summary>
        /// Gets or sets users latex path.
        /// </summary>
        /// <value>The latex path.</value>
        private string LatexPath { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server.
        /// </summary>
        /// <value>The SMTP server.</value>
        private string SmtpServer { get; set; }

        /// <summary>
        /// Gets or sets the SMTP user.
        /// </summary>
        /// <value>The SMTP user.</value>
        private string SmtpUser { get; set; }

        /// <summary>
        /// Gets or sets the SMTP pass.
        /// </summary>
        /// <value>The SMTP pass.</value>
        private string SmtpPass { get; set; }

        /// <summary>
        /// Gets or sets the SMTP port.
        /// </summary>
        /// <value>The SMTP port.</value>
        private int SmtpPort { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server option.
        /// </summary>
        /// <value>The SMTP server option.</value>
        private string SmtpServerOption { get; set; }

        /// <summary>
        /// Gets or sets the application path, where the application resources is installed.
        /// </summary>
        /// <value>The application path.</value>
        private string ApplicationPath { get; set; }

        protected async Task OnInitialized(IUserRepository repository)
        {
            var users = await repository.GetAllAsync();
            int count = users.Count();
            if (count == 0)
            {
                this.Firstname = "Max";
                this.Surname = "Mustermann";
                this.Street = "Musterstraße 1";
                this.City = "Musterstadt";
                this.Postcode = "12345";
                this.Phone = "0123456789";
                this.Email = "max.mustermann@example.com";
                this.Homepage = "https://www.example.com";
                this.BitlyApiKey = "YOUR_BITLY_API_KEY";
                this.LatexPath = "/usr/local/texlive/2023/bin/x86_64-linux";
                this.SmtpServer = "smtp.example.com";
                this.SmtpUser = "your_smtp_user";
                this.SmtpPass = "your_smtp_password";
                this.SmtpPort = 587;
                this.SmtpServerOption = "smtp.example.com";
                this.ApplicationPath = "/app/path";
            }
            else
            {
                var model = new User
                {
                    Firstname = this.Firstname,
                    Surname = this.Surname,
                    Street = this.Street,
                    City = this.City,
                    Postcode = this.Postcode,
                    Phone = this.Phone,
                    Email = this.Email,
                    Homepage = this.Homepage,
                    BitlyApiKey = this.BitlyApiKey,
                    LatexPath = this.LatexPath,
                    SmtpServer = this.SmtpServer,
                    SmtpUser = this.SmtpUser,
                    SmtpPass = this.SmtpPass,
                    SmtpPort = this.SmtpPort,
                    SmtpServerOption = this.SmtpServerOption,
                    ApplicationPath = this.ApplicationPath
                };
                await repository.AddAsync(model);
            }
        }
    }
}
