// <copyright file="MainLayout.razor.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.



namespace JobApplicationManager.Components.Shared
{
    using BootstrapBlazor.Components;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Routing;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// Main layout component for the application.
    /// </summary>
    public sealed partial class MainLayout()
    {
        [Inject]
        private IStringLocalizer<MainLayout> Loc { get; set; } = default!;

        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = string.Empty;

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = true;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem>? Menus { get; set; }

        public string UserSettingsTitle { get; set; }

        /// <summary>
        /// OnInitialized.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            UserSettingsTitle = Loc["UserSettingsTitle"];
            var menus = new List<MenuItem>
                            {
                                new() { Text = "返回组件库", Icon = "fa-solid fa-fw fa-home", Url = "https://www.blazor.zone/components" },
                                new() { Text = "Index", Icon = "fa-solid fa-fw fa-flag", Url = "/" , Match = NavLinkMatch.All},
                                new() { Text = "Counter", Icon = "fa-solid fa-fw fa-check-square", Url = "/counter" },
                                new() { Text = "Weather", Icon = "fa-solid fa-fw fa-database", Url = "/weather" },
                                new() { Text = "Table", Icon = "fa-solid fa-fw fa-table", Url = "/table" },
                                new() { Text = "花名册", Icon = "fa-solid fa-fw fa-users", Url = "/users" },
                                new() { Text = UserSettingsTitle, Icon = "fa-solid fa-fw fa-user-cog", Url = "/usersettings" },
                            };
            Menus = menus;

        }
    }
}
