// <copyright file="FileController.cs" company="Sascha Manns">
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System.ComponentModel.DataAnnotations;

namespace JobApplicationManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController(IFileStorage fileStorage, ILogger<FileController> logger, IOptions<AppSettings> settings,
    IMemoryCache cache, IFileNameGenerator fileNamaeGen)
    : ControllerBase
{
    private readonly FileStorage _fileStorageSettings = settings.Value.FileStorage;

    [HttpGet(@"{filename:regex((?!-)([[a-z0-9-]]+)\.(pdf))}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> File([MaxLength(256)] string filename)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        if (filename.IndexOfAny(invalidChars) >= 0)
        {
            logger.LogError("Invalid file name: {FileName}", filename);
            return BadRequest("Invalid file name.");
        }

        var file = await cache.GetOrCreateAsync(filename, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(_fileStorageSettings.CacheMinutes);
            var fileInfo = await fileStorage.GetAsync(filename);
            return fileInfo;
        });

        if (null == file) return NotFound();

        return File(file.FileBytes, file.FileContentType);
    }

    [Authorize]
    [HttpPost, IgnoreAntiforgeryToken]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> File([Required] IFormFile file)
    {
        var name = Path.GetFileName(file.FileName);
        var ext = Path.GetExtension(name).ToLower();
        string[] allowedExtensions = { ".pdf" };

        if (!allowedExtensions.Contains(ext))
        {
            return BadRequest("Invalid file type. Only PDF files are allowed.");
        }

        var primaryFileName = fileNamaeGen.GetFileName(name);

        await using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;

        var finalName = await fileStorage.InsertAsync(primaryFileName, stream.ToArray());

        logger.LogInformation($"File '{primaryFileName}' uploaded.");
        return Ok(new { location = $"/file/{finalName}", filename = $"/file/{finalName}" });
    }
}