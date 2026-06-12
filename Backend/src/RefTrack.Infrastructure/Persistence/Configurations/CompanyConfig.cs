using Microsoft.EntityFrameworkCore;                    // ToTable, HasKey, Property
using Microsoft.EntityFrameworkCore.Metadata.Builders;  // EntityTypeBuilder<T>
using RefTrack.Domain.Entities;

namespace RefTrack.Infrastructure.Persistence.Configurations
{
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> b)
        {
            b.ToTable("companies");          // maps to Supabase table name

            b.HasKey(c => c.Id);

            b.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            b.Property(c => c.Tier)
                .HasConversion<string>();    // stores enum as string in DB

            b.Property(c => c.CreatedAt)
                .HasDefaultValueSql("now()"); // PostgreSQL function
        }
    }
}
