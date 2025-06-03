// <copyright file="MainLayout.razor.cs" company="Sascha Manns">
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

using BootstrapBlazor.Components;

using JobApplicationManager.Resources.Localize;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;

namespace JobApplicationManager.Components.Shared
{
    /// <summary>
    /// Main layout component for the application.
    /// </summary>
    public sealed partial class MainLayout()
    {
        [Inject]
        private IStringLocalizer<SharedResource> Localizer { get; set; } = default!;

        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = string.Empty;

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem>? Menus { get; set; }

        public string UserSettingsTitle { get; set; } = string.Empty;

        /// <summary>
        /// OnInitialized.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.UserSettingsTitle = this.Localizer["UserSettingsTitle"];
            var menus = new List<MenuItem>
                            {
                                new() { Text = "Index", Icon = "fa-solid fa-fw fa-flag", Url = "/", Match = NavLinkMatch.All },
                                new() { Text = this.UserSettingsTitle, Icon = "fa-solid fa-fw fa-user-cog", Url = "/settings" }
                            };
            this.Menus = menus;
        }
    }
}