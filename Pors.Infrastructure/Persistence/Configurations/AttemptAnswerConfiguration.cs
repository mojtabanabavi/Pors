using System;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pors.Infrastructure.Persistence.Configurations
{
    public class AttemptAnswerConfiguration : IEntityTypeConfiguration<AttemptAnswer>
    {
        public void Configure(EntityTypeBuilder<AttemptAnswer> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder
                .HasOne(x => x.Option)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.OptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Question)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Attempt)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.AttemptId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
