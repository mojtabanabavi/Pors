using System;
using Pors.Domain.Entities;
using System.Threading.Tasks;
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
        DbSet<ExamQuestion> ExamQuestions { get; set; }
        DbSet<QuestionOption> QuestionOptions { get; set; }
        DbSet<ExamVisit> ExamVisits { get; set; }

        Task<int> SaveChangesAsync();
    }
}