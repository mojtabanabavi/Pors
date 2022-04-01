using System;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pors.Infrastructure.Persistence.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.HasIndex(x => x.Title);

            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ShortDescription)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Image)
                .HasMaxLength(120);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("getdate()");

            // soft delete
            builder.Property<bool>("IsDeleted");
            builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
        }
    }
}
