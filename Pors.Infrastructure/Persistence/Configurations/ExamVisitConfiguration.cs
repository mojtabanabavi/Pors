using System;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pors.Infrastructure.Persistence.Configurations
{
    public class ExamVisitConfiguration : IEntityTypeConfiguration<ExamVisit>
    {
        public void Configure(EntityTypeBuilder<ExamVisit> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("getdate()");
        }
    }
}
