using System;
using Pors.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pors.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.FirstName)
                .HasMaxLength(25);

            builder.Property(x => x.LastName)
                .HasMaxLength(25);

            builder.Property(x => x.Email)
                .HasMaxLength(320)
                .IsUnicode();

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode();

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.ProfilePicture)
                .HasMaxLength(120);

            builder.Property(x => x.IsActive)
                .HasDefaultValueSql("1");

            builder.Property(x => x.RegisterDateTime)
                .HasDefaultValueSql("getdate()");
        }
    }
}
