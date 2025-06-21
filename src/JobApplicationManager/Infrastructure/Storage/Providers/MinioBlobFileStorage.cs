// <copyright file="MinioBlobFileStorage.cs" company="Sascha Manns">
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

using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace JobApplicationManager.Infrastructure.Storage.Providers;

public class MinioBlobFileStorage : IFileStorage
{
    public string Name => nameof(MinioBlobFileStorage);

    private readonly IMinioClient _client;
    private readonly string _bucketName;
    private readonly string _secondaryBucketName;

    private readonly ILogger<MinioBlobFileStorage> _logger;

    public MinioBlobFileStorage(ILogger<MinioBlobFileStorage> logger, MinioBlobConfiguration blobConfiguration)
    {
        _logger = logger;

        _client = new MinioClient()
            .WithEndpoint(blobConfiguration.EndPoint)
            .WithCredentials(blobConfiguration.AccessKey, blobConfiguration.SecretKey);
        if (blobConfiguration.WithSSL)
        {
            _client = _client.WithSSL();
        }
        _client.Build();

        _bucketName = blobConfiguration.BucketName;
        _secondaryBucketName = blobConfiguration.SecondaryBucketName;

        logger.LogInformation($"Created {nameof(MinioBlobFileStorage)} at {blobConfiguration.EndPoint}");
    }

    protected virtual async Task CreateBucketIfNotExists()
    {
        var arg = new BucketExistsArgs().WithBucket(_bucketName);
        if (!await _client.BucketExistsAsync(arg))
        {
            var arg1 = new MakeBucketArgs().WithBucket(_bucketName);
            await _client.MakeBucketAsync(arg1);
        }
    }

    public Task<string> InsertAsync(string fileName, byte[] fileBytes)
    {
        return InsertInternalAsync(fileName, fileBytes, _bucketName);
    }

    public async Task<string> InsertSecondaryAsync(string fileName, byte[] fileBytes)
    {
        if (string.IsNullOrWhiteSpace(_secondaryBucketName))
        {
            _logger.LogError("Secondary bucket is not configured.");
            return null;
        }

        return await InsertInternalAsync(fileName, fileBytes, _secondaryBucketName);
    }

    public async Task<string> InsertInternalAsync(string fileName, byte[] fileBytes, string bucketName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        _logger.LogInformation($"Uploading {fileName} to Minio Blob Storage.");

        await CreateBucketIfNotExists();

        await using var fileStream = new MemoryStream(fileBytes);

        var putObjectArg = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length);

        await _client.PutObjectAsync(putObjectArg);

        _logger.LogInformation($"Uploaded image file '{fileName}' to Minio Blob Storage.");

        return fileName;
    }

    public async Task DeleteAsync(string fileName)
    {
        if (await BlobExistsAsync(fileName))
        {
            await _client.RemoveObjectAsync(
                new RemoveObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName));
        }
    }

    private async Task<bool> BlobExistsAsync(string fileName)
    {
        // Make sure Blob Container exists.
        if (!await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName))) return false;

        try
        {
            var arg = new StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName);

            await _client.StatObjectAsync(arg);
        }
        catch (Exception e)
        {
            if (e is ObjectNotFoundException)
            {
                return false;
            }
            throw;
        }
        return true;
    }

    public async Task<FileInfo> GetAsync(string fileName)
    {
        await using var memoryStream = new MemoryStream();
        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new ArgumentException("File extension is empty");
        }

        var exists = await BlobExistsAsync(fileName);
        if (!exists)
        {
            _logger.LogWarning($"Blob {fileName} not exist.");
            return null;
        }

        var arg = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithCallbackStream(stream =>
            {
                stream?.CopyTo(memoryStream);
            });

        await _client.GetObjectAsync(arg);
        var arr = memoryStream.ToArray();

        var fileType = extension.Replace(".", string.Empty);
        var imageInfo = new FileInfo
        {
            FileBytes = arr,
            FileExtensionName = fileType
        };

        return imageInfo;
    }
}