using System;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pors.Infrastructure.Persistence.Configurations
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.Value)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.ExpireAt)
                .IsRequired();
        }
    }
}
