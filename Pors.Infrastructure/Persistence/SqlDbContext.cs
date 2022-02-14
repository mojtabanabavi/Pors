using System;
using System.Reflection;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Persistence
{
    public class SqlDbContext : DbContext, ISqlDbContext
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<UserToken> UserTokens => Set<UserToken>();

        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            builder.ApplyConfigurationsFromAssembly(executingAssembly);

            base.OnModelCreating(builder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
