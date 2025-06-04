// <copyright file="UserRepository.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.

// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN 
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using JobApplicationManager.Domain.Interfaces;
using JobApplicationManager.Infrastructure.Data.Models;
using JobApplicationManager.Infrastructure.Exceptions;

using Microsoft.EntityFrameworkCore;

using System.Diagnostics.CodeAnalysis;

namespace JobApplicationManager.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Class UserRepository.
    /// Implements the <see cref="IUserRepository" />
    /// </summary>
    /// <seealso cref="IUserRepository" />
    public class UserRepository : IUserRepository
    {
        [SuppressMessage(
            "StyleCop.CSharp.NamingRules",
            "SA1309:FieldNamesMustNotBeginWithUnderscore",
            Justification = "Reviewed. Suppression is OK here.")]
        private readonly JobApplicationManagerContext _context;

        private readonly ILogger<UserRepository> _logger;

        public UserRepository(JobApplicationManagerContext context, ILogger<UserRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<IEnumerable<User>>? GetAllAsync()
        {
            try
            {
                return await this._context.Users.ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync failed");
                throw new JamException($"GetAllAsync failed: {ex.Message}");
            }
        }

        public async Task<User?>? GetByEmailAsync(string email)
        {
            try
            {
                return await this._context.Users.FirstOrDefaultAsync((User u) => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetByEmailAsync failed");
                throw new JamException($"GetByEmailAsync failed: {ex.Message}");
            }

        }

        public async Task AddAsync(User user)
        {
            try
            {
                this._context.Users.Add(user);
                await this._context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "AddAsync failed.");
                throw new JamException($"AddAsync failed: {e.Message}");
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                this._context.Users.Update(user);
                await this._context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdateAsync failed.");
                throw new JamException($"UpdateAsync failed: {e.Message}");
            }
        }

        public async Task DeleteAsync(string? email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("Email cannot be null or empty.", nameof(email));
                }
                else
                {
                    var user = await this.GetByEmailAsync(email);
                    if (user != null)
                    {
                        this._context.Users.Remove(user);
                        await this._context.SaveChangesAsync();
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeleteAsync failed");
                throw new JamException("DeleteAsyncFailed");
            }
        }
    }
}