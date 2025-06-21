// <copyright file="UserCacheRepository.cs" company="Sascha Manns">
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

using JobApplicationManager.Domain.Entities;
using JobApplicationManager.Domain.Interfaces;

using Microsoft.Extensions.Caching.Memory;

namespace JobApplicationManager.Infrastructure.Data.Repositories;

public class UserCacheRepository : IUserRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly UserRepository _repository;

    public UserCacheRepository(
        IMemoryCache memoryCache,
        UserRepository repository)
    {
        _memoryCache = memoryCache;
        _repository = repository;
    }

    public Task<IEnumerable<User>>? GetAllAsync()
    {
        string cacheKey = "users";
        return _memoryCache.GetOrCreate(cacheKey, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            return _repository.GetAllAsync();
        });
    }

    [return: System.Diagnostics.CodeAnalysis.MaybeNull]
    public Task<User?> GetByEmailAsync(string email)
    {
        string cacheKey = $"user-{email}";
        return _memoryCache.GetOrCreate(cacheKey, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            return _repository.GetByEmailAsync(email);
        });
    }

    public async Task<User?>? GetByGuidAsync(string userGuid)
    {
        // Assuming userGuid is unique, we can use it as a cache key.
        string cacheKey = $"user-guid-{userGuid}";
        return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            return await _repository.GetByGuidAsync(userGuid);
        });
    }

    public async Task AddAsync(User user)
    {
        await this._repository.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        await this._repository.UpdateAsync(user);
    }

    public async Task DeleteAsync(string email)
    {
        var user = await this.GetByEmailAsync(email);
        if (user != null)
        {
            await this._repository.DeleteAsync(email);
        }
    }
}