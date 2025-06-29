using Microsoft.EntityFrameworkCore;

namespace RaceStrategyApp.Models {
    public class RaceStrategyContext: DbContext {

        public RaceStrategyContext() {

        }

        public DbSet<Race> Races { get; set; }
        public DbSet<RaceSeries> RaceSeries { get; set; }
        public DbSet<RaceProgress> RaceProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder mb) {
            mb.Entity<Race>().Property(p => p.Name).IsRequired().HasMaxLength(100);

            mb.Entity<RaceProgress>().HasAlternateKey(rp => rp.RaceId);
 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseLazyLoadingProxies()
            .UseSqlite(@"Data Source=RaceStrategyDB.db");
        }
    }
}
