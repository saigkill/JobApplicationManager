// <copyright file="JobApplicationManagerContext.cs" company="Sascha Manns">
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

using Microsoft.EntityFrameworkCore;

namespace JobApplicationManager.Infrastructure.Data;

public partial class JobApplicationManagerContext : DbContext
{
    public JobApplicationManagerContext(DbContextOptions<JobApplicationManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<JobApplication> JobApplications { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Companies)
                .HasPrincipalKey(p => p.UserGuid)
                .HasForeignKey(d => d.UserGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Company_User");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Contacts)
                .HasPrincipalKey(p => p.UserGuid)
                .HasForeignKey(d => d.UserGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contact_Company");
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasOne(d => d.Company).WithMany(p => p.JobApplications).HasConstraintName("FK_JobApplication_Company");

            entity.HasOne(d => d.User).WithMany(p => p.JobApplications)
                .HasPrincipalKey(p => p.UserGuid)
                .HasForeignKey(d => d.UserGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobApplication_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}