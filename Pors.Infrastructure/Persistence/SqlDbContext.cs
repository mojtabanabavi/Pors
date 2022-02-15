﻿using System;
using System.Reflection;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;
using Pors.Infrastructure.Persistence.Configurations;

namespace Pors.Infrastructure.Persistence
{
    public class SqlDbContext : DbContext, ISqlDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
