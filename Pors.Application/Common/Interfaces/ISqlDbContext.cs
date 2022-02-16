using System;
using Pors.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pors.Application.Common.Interfaces
{
    public interface ISqlDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<UserRole> UserRoles { get; }
        DbSet<UserToken> UserTokens { get; }
        DbSet<Exam> Exams { get; }

        Task<int> SaveChangesAsync();
    }
}
