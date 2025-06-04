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
