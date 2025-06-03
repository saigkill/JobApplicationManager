// <copyright file="SettingsModel.cs" company="Sascha Manns">
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

using JobApplicationManager.Infrastructure.Models;
using JobApplicationManager.Resources.Localize;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace JobApplicationManager.Components.Models
{
    /// <summary>
    /// Class UserSettingsModel. 
    /// </summary>
    public class SettingsModel
    {
        /// <summary>
        /// Gets or sets users FirstName.
        /// </summary>
        /// <value>The FirstName.</value>
        [Required]
        [Display(Name = "Firstname", ResourceType = typeof(SharedResource))]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets users surname.
        /// </summary>
        /// <value>The surname.</value>
        [Required]
        [Display(Name = "Surname", ResourceType = typeof(SharedResource))]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Surname { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets users street.
        /// </summary>
        /// <value>The street.</value>
        [Required]
        [Display(Name = "Street", ResourceType = typeof(SharedResource))]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets users city.
        /// </summary>
        /// <value>The city.</value>
        [Required]
        [Display(Name = "City", ResourceType = typeof(SharedResource))]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets users postcode.
        /// </summary>
        /// <value>The postcode.</value>
        [Required]
        [Display(Name = "Postcode", ResourceType = typeof(SharedResource))]
        [StringLength(10)]
        [DataType(DataType.PostalCode)]
        public string Postcode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets users phone.
        /// </summary>
        /// <value>The phone.</value>
        [Required]
        [Display(Name = "Phone", ResourceType = typeof(SharedResource))]
        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets users email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [Display(Name = "Email", ResourceType = typeof(SharedResource))]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets users homepage.
        /// </summary>
        /// <value>The homepage.</value>
        [Display(Name = "Homepage", ResourceType = typeof(SharedResource))]
        [StringLength(255)]
        [DataType(DataType.Url)]
        public string? Homepage { get; set; }

        /// <summary>
        /// Gets or sets users bitly API key.
        /// </summary>
        /// <value>The bitly API key.</value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [Display(Name = "BitlyApiKey", ResourceType = typeof(SharedResource))]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string? BitLyApiKey { get; set; }

        /// <summary>
        /// Gets or sets users latex path.
        /// </summary>
        /// <value>The latex path.</value>
        [Required]
        [Display(Name = "LatexPath", ResourceType = typeof(SharedResource))]
        [StringLength(200)]
        [DataType(DataType.Text)]
        public string LatexPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SMTP server.
        /// </summary>
        /// <value>The SMTP server.</value>
        [Required]
        [Display(Name = "SmtpServer", ResourceType = typeof(SharedResource))]
        [StringLength(255)]
        [DataType(DataType.Url)]
        public string SmtpServer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SMTP user.
        /// </summary>
        /// <value>The SMTP user.</value>
        [Required]
        [Display(Name = "SmtpUser", ResourceType = typeof(SharedResource))]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string SmtpUser { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SMTP pass.
        /// </summary>
        /// <value>The SMTP pass.</value>
        [Required]
        [Display(Name = "SmtpPass", ResourceType = typeof(SharedResource))]
        [DataType(DataType.Password)]
        [StringLength(20)]
        public string SmtpPass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SMTP port.
        /// </summary>
        /// <value>The SMTP port.</value>
        [Required]
        [Display(Name = "SmtpPort", ResourceType = typeof(SharedResource))]
        public int SmtpPort { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server option.
        /// </summary>
        /// <value>The SMTP server option.</value>
        [Required]
        [Display(Name = "SmtpServerOption", ResourceType = typeof(SharedResource))]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string SmtpServerOption { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email options. It's for the picker component to select the SMTP server option.
        /// </summary>
        /// <value>The email options.</value>
        public List<PickerEmailOptionsModel> EmailOptions { get; set; } = new PickerEmailOptionsModel().GetOptions();
    }
}