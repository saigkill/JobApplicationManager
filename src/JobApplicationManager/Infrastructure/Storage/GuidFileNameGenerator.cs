// <copyright file="GuidFileNameGenerator.cs" company="Sascha Manns">
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

namespace JobApplicationManager.Infrastructure.Storage;

public class GuidFileNameGenerator(Guid id) : IFileNameGenerator
{
    public string Name => nameof(GuidFileNameGenerator);

    public Guid UniqueId { get; } = id;

    public string GetFileName(string fileName, string appendixName = "")
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var ext = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(ext) ||
            string.IsNullOrWhiteSpace(Path.GetFileNameWithoutExtension(fileName)))
        {
            throw new ArgumentException("Invalid File Name", nameof(fileName));
        }

        var newFileName = $"file-{UniqueId}{(string.IsNullOrWhiteSpace(appendixName) ? string.Empty : "-" + appendixName)}{ext}".ToLower();
        return newFileName;
    }
}