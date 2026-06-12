using Microsoft.EntityFrameworkCore;
using RefTrack.Domain.Entities;
using RefTrack.Infrastructure.Persistence.Configurations;
using AppEntity = RefTrack.Domain.Entities.Application;

namespace RefTrack.Infrastructure.Persistence
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
       : base(options) { }

        // ── DbSets = one per entity ────────────────────────
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }
        public DbSet<Referrer> Referrers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        // Application collides with the namespace — use alias
        public DbSet<AppEntity> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // Applies all IEntityTypeConfiguration files
            // in this assembly automatically
            mb.ApplyConfigurationsFromAssembly(
                typeof(AppDBContext).Assembly);
        }
    }
}
