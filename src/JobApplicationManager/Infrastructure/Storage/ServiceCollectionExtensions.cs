// <copyright file="ServiceCollectionExtensions.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using JobApplicationManager.Application.Interfaces;
using JobApplicationManager.Application.Models;
using JobApplicationManager.Infrastructure.Storage.Providers;

namespace JobApplicationManager.Infrastructure.Storage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(AppSettings));
        var settings = section.Get<AppSettings>();

        if (section == null)
        {
            throw new ArgumentNullException(nameof(section), "ImageStorage settings cannot be null.");
        }

        var provider = settings.FileStorage.Provider?.ToLower();
        if (string.IsNullOrWhiteSpace(provider))
        {
            throw new ArgumentNullException("Provider", "Provider can not be empty.");
        }

        switch (provider)
        {
            case "azurestorage":
                if (settings.FileStorage.AzureStorageSettings == null)
                {
                    throw new ArgumentNullException(nameof(settings.FileStorage.AzureStorageSettings), "AzureStorageSettings can not be null.");
                }
                services.AddAzureStorage(settings.FileStorage.AzureStorageSettings);
                break;
            case "filesystem":
                if (string.IsNullOrWhiteSpace(settings.FileStorage.FileSystemPath))
                {
                    throw new ArgumentNullException(nameof(settings.FileStorage.FileSystemPath), "FileSystemPath can not be null or empty.");
                }
                services.AddFileSystemStorage(settings.FileStorage.FileSystemPath);
                break;
            case "miniostorage":
                if (settings.FileStorage.MinioStorageSettings == null)
                {
                    throw new ArgumentNullException(nameof(settings.FileStorage.MinioStorageSettings), "MinioStorageSettings can not be null.");
                }
                services.AddMinioStorage(settings.FileStorage.MinioStorageSettings);
                break;
            default:
                var msg = $"Provider {provider} is not supported.";
                throw new NotSupportedException(msg);
        }

        return services;
    }

    private static void AddAzureStorage(this IServiceCollection services, AzureStorageSettings settings)
    {
        var conn = settings.ConnectionString;
        var container = settings.ContainerName;
        var secondaryContainer = settings.SecondaryContainerName;
        services.AddSingleton(_ => new AzureBlobConfiguration(conn, container, secondaryContainer))
                .AddSingleton<IFileStorage, AzureFileStorage>()
                .AddScoped<IFileNameGenerator>(_ => new GuidFileNameGenerator(Guid.NewGuid()));
    }

    private static void AddMinioStorage(this IServiceCollection services, MinioStorageSettings settings)
    {
        services.AddSingleton<IFileStorage, MinioBlobFileStorage>()
            .AddScoped<IFileNameGenerator>(_ => new GuidFileNameGenerator(Guid.NewGuid()))
            .AddSingleton(_ => new MinioBlobConfiguration(
                settings.EndPoint,
                settings.AccessKey,
                settings.SecretKey,
                settings.BucketName,
                settings.SecondaryBucketName,
                settings.WithSSL));
    }
    private static void AddFileSystemStorage(this IServiceCollection services, string fileSystemPath)
    {
        var fullPath = FileSystemFileStorage.ResolveImageStoragePath(fileSystemPath);
        services.AddSingleton(_ => new FileSystemFileConfiguration(fullPath))
                .AddSingleton<IFileStorage, FileSystemFileStorage>()
                .AddScoped<IFileNameGenerator>(_ => new GuidFileNameGenerator(Guid.NewGuid()));
    }
}