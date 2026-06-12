using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefTrack.Domain.Entities;

namespace RefTrack.Infrastructure.Persistence.Configurations
{
    internal class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> b)
        {
            b.ToTable("app_users");

            b.HasKey(u => u.Id);

            b.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            b.HasIndex(u => u.Email)
                .IsUnique();              // DB-level uniqueness constraint

            b.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(512);

            b.Property(u => u.Role)
                .HasConversion<string>()  // "Member" or "Admin"
                .HasMaxLength(20);

            b.Property(u => u.CreatedAt)
                .HasDefaultValueSql("now()");

            b.Property(u => u.UpdatedAt)
                .HasDefaultValueSql("now()");
        }
    }
}
