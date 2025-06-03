// <copyright file="Program.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.

// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN 
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using JobApplicationManager.Application.Models;
using JobApplicationManager.Components;
using JobApplicationManager.Infrastructure.Data;
using JobApplicationManager.Infrastructure.Data.Repositories;
using JobApplicationManager.Infrastructure.Helpers;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

using NLog;
using NLog.Web;

using Saigkill.Toolbox.Services;

using Syncfusion.Blazor;

using System.Text;

namespace JobApplicationManager;

/// <summary>
/// Main entry point for the application.
/// </summary>
public static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("init main");

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddBootstrapBlazor();

        AddServices(builder);

        ConfigureDatabase(builder.Services, builder.Configuration);

        ConfigureSyncfusion(builder);

        builder.Services.Configure<HubOptions>((HubOptions option) => option.MaximumReceiveMessageSize = null);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseResponseCompression();
        }

        string[] supportedCultures = ["en-US", "de-DE"];
        var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures).AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();

        DoSetup();

        app.UseAntiforgery();

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        app.Run();
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<JobApplicationManagerContext>((DbContextOptionsBuilder options) =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICsvService, CsvService>();
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<IUserRepository, UserCacheRepository>();
        builder.Services.AddLocalization();
        builder.Services.AddSyncfusionBlazor();
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddOptions<AppSettings>().Bind(builder.Configuration);

        builder.Host.UseNLog();
    }

    private static void ConfigureSyncfusion(WebApplicationBuilder builder)
    {
        if (builder.Configuration["SyncfusionLicenseKey"] is not null)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["SyncfusionLicenseKey"]);
        }
    }

    private static void DoSetup()
    {
        Setup.CheckDocumentsPath();
    }
}