namespace JobApplicationManager.Application.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;

public class AzureAd
{
    public string Instance { get; set; }
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string CallbackPath { get; set; }
}

public class AzureStorageSettings
{
    public string ConnectionString { get; set; }
    public string ContainerName { get; set; }
    public string SecondaryContainerName { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
}

public class EmailServer
{
    public string DefaultEmailAddress { get; set; }
    public string DefaultSenderName { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public bool UseSSL { get; set; }
    public string Host { get; set; }
}

public class FileStorage
{
    public List<string> AllowedExtensions { get; set; }
    public int CacheMinutes { get; set; }
    public string Provider { get; set; }
    public string FileSystemPath { get; set; }
    public AzureStorageSettings AzureStorageSettings { get; set; }
    public MinioStorageSettings MinioStorageSettings { get; set; }
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

public class MinioStorageSettings
{
    public string EndPoint { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }
    public string SecondaryBucketName { get; set; }
    public bool WithSSL { get; set; }
}

public class AppSettings
{
    public Logging Logging { get; set; }
    public string AllowedHosts { get; set; }
    public EmailServer EmailServer { get; set; }
    public string Environment { get; set; }
    public string SyncfusionLicenseKey { get; set; }
    public AzureAd AzureAd { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public FileStorage FileStorage { get; set; }
}

