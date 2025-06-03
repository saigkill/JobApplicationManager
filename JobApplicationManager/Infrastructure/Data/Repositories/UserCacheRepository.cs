using JobApplicationManager.Infrastructure.Data.Models;

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

    public Task<User?>? GetByEmailAsync(string email)
    {
        string cacheKey = $"user-{email}";
        return _memoryCache.GetOrCreate(cacheKey, entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            return _repository.GetByEmailAsync(email);
        });
    }

    /// <summary>
    /// Add as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task AddAsync(User user)
    {
        await this._repository.AddAsync(user);
    }

    /// <summary>
    /// Update as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task UpdateAsync(User user)
    {
        await this._repository.UpdateAsync(user);
    }

    /// <summary>
    /// Delete as an asynchronous operation.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task DeleteAsync(string email)
    {
        var user = await this.GetByEmailAsync(email);
        if (user != null)
        {
            await this._repository.DeleteAsync(email);
        }
    }
}
