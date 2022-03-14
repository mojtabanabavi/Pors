using System;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pors.Infrastructure.Persistence.Configurations
{
    public class FaqConfiguration : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.Question)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.Answer)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("getdate()");
        }
    }
}
