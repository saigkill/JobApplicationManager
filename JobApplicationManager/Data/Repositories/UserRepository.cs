// <copyright file="UserRepository.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.

namespace JobApplicationManager.Data.Repositories
{
    using JobApplicationManager.Data.Models;

    using Microsoft.EntityFrameworkCore;

    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Interface IUserRepository
    /// </summary>
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string email);
    }

    /// <summary>
    /// Class UserRepository.
    /// Implements the <see cref="JobApplicationManager.Data.Repositories.IUserRepository" />
    /// </summary>
    /// <seealso cref="JobApplicationManager.Data.Repositories.IUserRepository" />
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private readonly JobApplicationManagerContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserRepository(JobApplicationManagerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Get by email as an asynchronous operation.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A Task&lt;User&gt; representing the asynchronous operation.</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Add as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task AddAsync(User user)
        {
            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Update as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete as an asynchronous operation.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(string email)
        {
            var user = await GetByEmailAsync(email);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}