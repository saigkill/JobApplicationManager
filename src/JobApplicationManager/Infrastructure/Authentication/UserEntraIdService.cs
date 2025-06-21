// <copyright file="UserEntraIdService.cs" company="Sascha Manns">
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

using JobApplicationManager.Application.Models;
using JobApplicationManager.Domain.Interfaces;

using Microsoft.Extensions.Options;

namespace JobApplicationManager.Infrastructure.Authentication;

public class UserEntraIdService(IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> settings) : IUserEntraIdService
{
    public string? GetUserId()
    {
        if (settings.Value.Environment == "development")
        {
            // For development purposes, return a fixed user ID.
            return "4b689bfe-cfa0-44ec-8447-79a62871b078"; // Example GUID for development
        }
        var user = httpContextAccessor.HttpContext?.User;
        return user?.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        // Alternativ: return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}