using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefTrack.Domain.Entities;
using AppEntity = RefTrack.Domain.Entities.Application;

namespace RefTrack.Infrastructure.Persistence.Configurations
{
    public class ApplicationConfig : IEntityTypeConfiguration<AppEntity>  // use alias here
    {
        public void Configure(EntityTypeBuilder<AppEntity> b)
        {
            b.ToTable("applications");
            b.HasKey(a => a.Id);

            b.Property(a => a.Status)
                .HasConversion<string>();

            b.Property(a => a.UpdatedAt)
                .HasDefaultValueSql("now()");

            // Foreign key to AppUser
            b.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(a => a.UserId);
        }
    }
}
