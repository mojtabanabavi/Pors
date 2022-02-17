using System;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pors.Infrastructure.Persistence.Configurations
{
    public class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
    {
        public void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.HasIndex(x => x.Title);

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
