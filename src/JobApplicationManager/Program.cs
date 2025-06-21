using JobApplicationManager.Application.EventHandlers;
using JobApplicationManager.Application.Interfaces;
using JobApplicationManager.Application.Models;
using JobApplicationManager.Components;
using JobApplicationManager.Domain.Abstractions;
using JobApplicationManager.Domain.Events;
using JobApplicationManager.Domain.Interfaces;
using JobApplicationManager.Infrastructure.Authentication;
using JobApplicationManager.Infrastructure.Data;
using JobApplicationManager.Infrastructure.Data.Repositories;
using JobApplicationManager.Infrastructure.Events;
using JobApplicationManager.Infrastructure.Helpers;
using JobApplicationManager.Infrastructure.Services;
using JobApplicationManager.Infrastructure.Storage;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

using NLog;
using NLog.Web;

using Saigkill.Toolbox.Services;

using Syncfusion.Blazor;

namespace JobApplicationManager;

public static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var logger = NLog.LogManager.Setup()
            .LoadConfigurationFromAppSettings()
            .GetCurrentClassLogger();
        logger.Debug("init main");

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddSyncfusionBlazor();
        builder.Services.AddControllers();
        builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));

        var supportedCultures = new[] { "en-US", "de", "fr", "ar", "zh" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        AddServices(builder);
        ConfigureDatabase(builder.Services, builder.Configuration);

        var app = builder.Build();
        //Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["SyncfusionLicenseKey"]);

        // Configure the HTTP request pipeline.
        app.UseRequestLocalization(localizationOptions);

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        DoSetup();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapControllers();
        app.Run();
    }

    private static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<AppSettings>().Bind(builder.Configuration);
        builder.Services.AddScoped<ICsvService, CsvService>();
        builder.Services.AddScoped<IEmailService, EmailServiceWithAuth>();
        builder.Services.AddScoped<ILatexBuildService, LatexBuildService>();
        builder.Services.AddScoped<IPdfCreationService, PdfCreationService>();
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<IUserRepository, UserCacheRepository>();
        builder.Services.AddScoped<IUserEntraIdService, UserEntraIdService>();
        builder.Services.AddFileStorage(builder.Configuration);
        builder.Services.AddLocalization();
        builder.Services.AddSyncfusionBlazor();
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        builder.Services.AddScoped<IDomainEventHandler<ApplicationDataReadyEvent>, BuildLatexDocumentsHandler>();
        builder.Services.AddHostedService<ReminderService>();
        builder.Services.AddHttpContextAccessor();
        builder.Host.UseNLog();
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<JobApplicationManagerContext>((DbContextOptionsBuilder options) =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }

    private static void DoSetup()
    {
        Setup.CheckDocumentsPath();
    }
}