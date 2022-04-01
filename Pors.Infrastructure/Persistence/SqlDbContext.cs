﻿using System;
using System.Reflection;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Pors.Infrastructure.Persistence
{
    public class SqlDbContext : DbContext, ISqlDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<AttemptAnswer> AttemptAnswers { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.ConfigureWarnings(x =>
                x.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));

            base.OnConfiguring(builder);
        }

        public async Task<int> SaveChangesAsync()
        {
            ApplySoftDeletes();

            return await base.SaveChangesAsync();
        }

        private void ApplySoftDeletes()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted &&
                    entry.CurrentValues.TryGetValue<bool>("IsDeleted", out _))
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                }
            }
        }
    }
}
