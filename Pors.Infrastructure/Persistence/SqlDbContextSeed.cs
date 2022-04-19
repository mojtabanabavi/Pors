using System;
using Loby.Tools;
using System.Linq;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;

namespace Pors.Infrastructure.Persistence
{
    public class SqlDbContextSeed : ISqlDbContextSeed
    {
        private readonly SqlDbContext _dbContext;
        private readonly ILogger<SqlDbContextSeed> _logger;

        public SqlDbContextSeed(SqlDbContext dbContext, ILogger<SqlDbContextSeed> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task SeedDataAsync()
        {
            try
            {
                // Create database and apply migrations
                await _dbContext.Database.MigrateAsync();

                await SeedDefaultUserAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred in the database seeding.", e);
            }
        }

        public async Task SeedDefaultUserAsync()
        {
            const string DefaultRoleName = "سوپر ادمین";

            var administrator = new User
            {
                IsActive = true,
                LastName = "نبوی",
                FirstName = "مجتبی",
                IsEmailConfirmed = true,
                PhoneNumber = "09104647055",
                IsPhoneNumberConfirmed = true,
                Email = "mj.nabawi@gmail.com",
                PasswordHash = PasswordHasher.Hash("nabavi123344"),
                UserRoles = new List<UserRole>
                {
                    new UserRole()
                    {
                        Role = new Role(DefaultRoleName)
                    }
                }
            };

            if (!_dbContext.Users.Any(u => u.Email == administrator.Email))
            {
                _dbContext.Users.Add(administrator);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
