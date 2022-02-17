using System;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pors.Infrastructure.Persistence.Configurations
{
    public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.HasIndex(x => x.Title);

            builder.Property(x => x.Title)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("getdate()");
        }
    }
}
