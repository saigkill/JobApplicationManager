// <copyright file="FileSystemFileStorage.cs" company="Sascha Manns">
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

using System.Runtime.InteropServices;

namespace JobApplicationManager.Infrastructure.Storage.Providers;

public class FileSystemFileStorage(FileSystemFileConfiguration fileConfig, ILogger<FileSystemFileStorage> logger) : IFileStorage
{
    public string Name => nameof(FileSystemFileStorage);

    private readonly string _path = fileConfig.Path;

    public async Task<FileInfo> GetAsync(string fileName)
    {
        var filePath = Path.Join(_path, fileName);

        if (!File.Exists(filePath))
        {
            // Can not throw FileNotFoundException,
            // because hackers may request a large number of 404 images
            // to flood .NET runtime with exceptions and take out the server
            return null;
        }

        var extension = Path.GetExtension(filePath);

        var fileType = extension.Replace(".", string.Empty);
        var imageBytes = await ReadFileAsync(filePath);

        var imageInfo = new FileInfo
        {
            FileBytes = imageBytes,
            FileExtensionName = fileType
        };

        return imageInfo;
    }

    public async Task DeleteAsync(string fileName)
    {
        await Task.CompletedTask;
        var imagePath = Path.Join(_path, fileName);
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
            logger.LogInformation("Deleted image: {FileName}", fileName);
        }
    }

    private static async Task<byte[]> ReadFileAsync(string filename)
    {
        await using var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        var buff = new byte[file.Length];
        await file.ReadAsync(buff.AsMemory(0, (int)file.Length));
        return buff;
    }

    public async Task<string> InsertAsync(string fileName, byte[] imageBytes)
    {
        var fullPath = Path.Join(_path, fileName);

        await using var sourceStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None,
            4096, true);
        await sourceStream.WriteAsync(imageBytes.AsMemory(0, imageBytes.Length));

        logger.LogInformation("Saved image: {FileName}", fileName);

        return fileName;
    }

    public Task<string> InsertSecondaryAsync(string fileName, byte[] imageBytes) => InsertAsync(fileName, imageBytes);

    public static string ResolveImageStoragePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        // Handle Path for non-Windows environment #412
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || Path.DirectorySeparatorChar != '\\')
        {
            if (path.IndexOf('\\') > 0)
            {
                path = path.Replace('\\', Path.DirectorySeparatorChar);
            }
        }

        // IsPathFullyQualified can't check if path is valid, e.g.:
        // Path: C:\Documents<>|foo
        //   Rooted: True
        //   Fully qualified: True
        //   Full path: C:\Documents<>|foo
        var invalidChars = Path.GetInvalidPathChars();
        if (path.IndexOfAny(invalidChars) >= 0)
        {
            throw new InvalidOperationException("Path can not contain invalid chars.");
        }
        if (!Path.IsPathFullyQualified(path))
        {
            throw new InvalidOperationException("Path is not fully qualified.");
        }

        var fullPath = Path.GetFullPath(path);
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
        return fullPath;
    }
}