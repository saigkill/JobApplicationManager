using Newtonsoft.Json;

namespace JobApplicationManager.Application.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class AzureAd
{
    public string Instance { get; set; }
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string CallbackPath { get; set; }
}

public class BootstrapBlazorOptions
{
    public int ToastDelay { get; set; }
    public int MessageDelay { get; set; }
    public int SwalDelay { get; set; }
    public bool EnableErrorLogger { get; set; }
    public string FallbackCulture { get; set; }
    public List<string> SupportedCultures { get; set; }
    public TableSettings TableSettings { get; set; }
    public bool IgnoreLocalizerMissing { get; set; }
    public StepSettings StepSettings { get; set; }
    public string DefaultCultureInfo { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}

public class Logging
{
    public LogLevel LogLevel { get; set; }
}

public class LogLevel
{
    public string Default { get; set; }

    [JsonProperty("Microsoft.AspNetCore")]
    public string MicrosoftAspNetCore { get; set; }
}

public class AppSettings
{
    public Logging Logging { get; set; }
    public string AllowedHosts { get; set; }
    public BootstrapBlazorOptions BootstrapBlazorOptions { get; set; }
    public string SyncfusionLicenseKey { get; set; }
    public AzureAd AzureAd { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class StepSettings
{
    public string Short { get; set; }
    public string Int { get; set; }
    public string Long { get; set; }
    public string Float { get; set; }
    public string Double { get; set; }
    public string Decimal { get; set; }
}

public class TableSettings
{
    public int CheckboxColumnWidth { get; set; }
}


